Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Threading.Tasks

''' <summary>
''' Classe pour envoyer des emails via SMTP (Gmail, Outlook, etc.)
''' Alternative à EmailSender (Resend API) pour plus de flexibilité
''' </summary>
Public Class EmailSenderSMTP
    Private ReadOnly _smtpHost As String
    Private ReadOnly _smtpPort As Integer
    Private ReadOnly _username As String
    Private ReadOnly _password As String
    Private ReadOnly _fromEmail As String
    Private ReadOnly _fromName As String
    Private ReadOnly _enableSsl As Boolean

    ''' <summary>
    ''' Initialise une nouvelle instance de EmailSenderSMTP avec configuration manuelle
    ''' </summary>
    ''' <param name="smtpHost">Serveur SMTP (ex: smtp.gmail.com)</param>
    ''' <param name="smtpPort">Port SMTP (ex: 587 pour TLS, 465 pour SSL)</param>
    ''' <param name="username">Nom d'utilisateur SMTP</param>
    ''' <param name="password">Mot de passe SMTP (ou mot de passe d'application)</param>
    ''' <param name="fromEmail">Adresse email de l'expéditeur</param>
    ''' <param name="fromName">Nom affiché de l'expéditeur</param>
    ''' <param name="enableSsl">Activer SSL/TLS (par défaut True)</param>
    Public Sub New(smtpHost As String, smtpPort As Integer, username As String, password As String, 
                   fromEmail As String, fromName As String, Optional enableSsl As Boolean = True)
        If String.IsNullOrWhiteSpace(smtpHost) Then Throw New ArgumentException("Le serveur SMTP ne peut pas être vide", NameOf(smtpHost))
        If smtpPort <= 0 Then Throw New ArgumentException("Le port SMTP doit être positif", NameOf(smtpPort))
        If String.IsNullOrWhiteSpace(username) Then Throw New ArgumentException("Le nom d'utilisateur ne peut pas être vide", NameOf(username))
        If String.IsNullOrWhiteSpace(password) Then Throw New ArgumentException("Le mot de passe ne peut pas être vide", NameOf(password))
        If String.IsNullOrWhiteSpace(fromEmail) Then Throw New ArgumentException("L'email expéditeur ne peut pas être vide", NameOf(fromEmail))
        If String.IsNullOrWhiteSpace(fromName) Then Throw New ArgumentException("Le nom expéditeur ne peut pas être vide", NameOf(fromName))

        _smtpHost = smtpHost
        _smtpPort = smtpPort
        _username = username
        _password = password
        _fromEmail = fromEmail
        _fromName = fromName
        _enableSsl = enableSsl
    End Sub

    ''' <summary>
    ''' Initialise une nouvelle instance de EmailSenderSMTP avec configuration automatique depuis .env
    ''' Charge automatiquement les paramètres SMTP depuis les variables d'environnement
    ''' </summary>
    Public Sub New()
        ' Chargement automatique de la configuration depuis .env
        EnvConfig.LoadEnvFile()
        
        _smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST")
        _smtpPort = Integer.Parse(Environment.GetEnvironmentVariable("SMTP_PORT"))
        _username = Environment.GetEnvironmentVariable("SMTP_USERNAME")
        _password = Environment.GetEnvironmentVariable("SMTP_PASSWORD")
        _fromEmail = Environment.GetEnvironmentVariable("SMTP_FROM_EMAIL")
        _fromName = Environment.GetEnvironmentVariable("SMTP_FROM_NAME")
        _enableSsl = True
        
        ' Validation
        If String.IsNullOrWhiteSpace(_smtpHost) Then Throw New InvalidOperationException("SMTP_HOST non défini dans .env")
        If String.IsNullOrWhiteSpace(_username) Then Throw New InvalidOperationException("SMTP_USERNAME non défini dans .env")
        If String.IsNullOrWhiteSpace(_password) Then Throw New InvalidOperationException("SMTP_PASSWORD non défini dans .env")
        If String.IsNullOrWhiteSpace(_fromEmail) Then Throw New InvalidOperationException("SMTP_FROM_EMAIL non défini dans .env")
        If String.IsNullOrWhiteSpace(_fromName) Then Throw New InvalidOperationException("SMTP_FROM_NAME non défini dans .env")
    End Sub

    ''' <summary>
    ''' Envoie un email avec configuration automatique depuis .env (tous les paramètres optionnels)
    ''' </summary>
    ''' <param name="destinataire">Destinataire (optionnel si défini dans EMAIL_TO)</param>
    ''' <param name="sujet">Sujet (optionnel si défini dans EMAIL_SUBJECT)</param>
    ''' <param name="message">Message HTML (requis)</param>
    ''' <param name="typeEmail">Type d'email (optionnel, utilise EMAIL_TYPE de .env si Nothing)</param>
    ''' <param name="signature">Signature (optionnel, utilise EMAIL_SIGNATURE de .env si Nothing)</param>
    ''' <param name="pieceJointes">Noms de pièces jointes à afficher (optionnel)</param>
    ''' <param name="fichiersAttaches">Chemins des fichiers à attacher (optionnel, utilise EMAIL_ATTACHMENTS de .env si Nothing)</param>
    ''' <param name="cc">Destinataires en copie (optionnel, utilise EMAIL_CC de .env si Nothing)</param>
    ''' <param name="cci">Destinataires en copie cachée (optionnel, utilise EMAIL_BCC de .env si Nothing)</param>
    ''' <returns>True si l'email est envoyé avec succès, False sinon</returns>
    Public Async Function EnvoyerEmailAsync(
        message As String,
        Optional destinataire As String = Nothing,
        Optional sujet As String = Nothing,
        Optional typeEmail As TypeEmail? = Nothing,
        Optional signature As String = Nothing,
        Optional pieceJointes As List(Of String) = Nothing,
        Optional fichiersAttaches As List(Of String) = Nothing,
        Optional cc As List(Of String) = Nothing,
        Optional cci As List(Of String) = Nothing
    ) As Task(Of Boolean)

        Try
            ' Chargement automatique des valeurs depuis .env si non fournies
            If String.IsNullOrWhiteSpace(destinataire) Then
                destinataire = Environment.GetEnvironmentVariable("EMAIL_TO")
            End If
            
            If String.IsNullOrWhiteSpace(sujet) Then
                sujet = Environment.GetEnvironmentVariable("EMAIL_SUBJECT")
                If String.IsNullOrWhiteSpace(sujet) Then
                    sujet = "Email depuis EmailSenderDLL"
                End If
            End If
            
            If Not typeEmail.HasValue Then
                Dim typeStr = Environment.GetEnvironmentVariable("EMAIL_TYPE")
                If Not String.IsNullOrWhiteSpace(typeStr) Then
                    Dim parsedType As TypeEmail
                    If [Enum].TryParse(typeStr, True, parsedType) Then
                        typeEmail = parsedType
                    Else
                        typeEmail = EmailSenderDLL.TypeEmail.Info
                    End If
                Else
                    typeEmail = EmailSenderDLL.TypeEmail.Info
                End If
            End If
            
            If String.IsNullOrWhiteSpace(signature) Then
                signature = Environment.GetEnvironmentVariable("EMAIL_SIGNATURE")
            End If
            
            ' Chargement des pièces jointes depuis .env si non fournies
            If fichiersAttaches Is Nothing OrElse fichiersAttaches.Count = 0 Then
                Dim attachmentsStr = Environment.GetEnvironmentVariable("EMAIL_ATTACHMENTS")
                If Not String.IsNullOrWhiteSpace(attachmentsStr) Then
                    fichiersAttaches = attachmentsStr.Split(";"c).Where(Function(s) Not String.IsNullOrWhiteSpace(s)).ToList()
                End If
            End If
            
            ' Chargement des CC depuis .env si non fournis
            If cc Is Nothing OrElse cc.Count = 0 Then
                Dim ccStr = Environment.GetEnvironmentVariable("EMAIL_CC")
                If Not String.IsNullOrWhiteSpace(ccStr) Then
                    cc = ccStr.Split(";"c).Where(Function(s) Not String.IsNullOrWhiteSpace(s)).ToList()
                End If
            End If
            
            ' Chargement des BCC depuis .env si non fournis
            If cci Is Nothing OrElse cci.Count = 0 Then
                Dim bccStr = Environment.GetEnvironmentVariable("EMAIL_BCC")
                If Not String.IsNullOrWhiteSpace(bccStr) Then
                    cci = bccStr.Split(";"c).Where(Function(s) Not String.IsNullOrWhiteSpace(s)).ToList()
                End If
            End If
            
            ' Appel de la méthode complète
            Return Await EnvoyerEmailCompletAsync(destinataire, sujet, message, typeEmail.Value, signature, pieceJointes, fichiersAttaches, cc, cci)
            
        Catch ex As Exception
            Console.WriteLine($"Exception lors de l'envoi de l'email: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Envoie un email avec template personnalisé via SMTP (méthode complète)
    ''' </summary>
    ''' <param name="destinataire">Adresse email du destinataire principal</param>
    ''' <param name="sujet">Sujet de l'email</param>
    ''' <param name="message">Contenu du message (HTML autorisé)</param>
    ''' <param name="typeEmail">Type d'email (Info, Erreur, Urgence, Succes, etc.)</param>
    ''' <param name="signature">Signature personnalisée (optionnel)</param>
    ''' <param name="pieceJointes">Liste des noms de pièces jointes à afficher (optionnel)</param>
    ''' <param name="fichiersAttaches">Liste des chemins complets des fichiers à attacher (optionnel)</param>
    ''' <param name="cc">Liste des destinataires en copie (optionnel)</param>
    ''' <param name="cci">Liste des destinataires en copie cachée (optionnel)</param>
    ''' <returns>True si l'email est envoyé avec succès, False sinon</returns>
    Public Async Function EnvoyerEmailCompletAsync(
        destinataire As String,
        sujet As String,
        message As String,
        typeEmail As TypeEmail,
        Optional signature As String = Nothing,
        Optional pieceJointes As List(Of String) = Nothing,
        Optional fichiersAttaches As List(Of String) = Nothing,
        Optional cc As List(Of String) = Nothing,
        Optional cci As List(Of String) = Nothing
    ) As Task(Of Boolean)

        Try
            ' Validation des paramètres
            If String.IsNullOrWhiteSpace(destinataire) Then
                Throw New ArgumentException("Le destinataire ne peut pas être vide", NameOf(destinataire))
            End If
            If String.IsNullOrWhiteSpace(sujet) Then
                Throw New ArgumentException("Le sujet ne peut pas être vide", NameOf(sujet))
            End If
            If String.IsNullOrWhiteSpace(message) Then
                Throw New ArgumentException("Le message ne peut pas être vide", NameOf(message))
            End If

            ' Génération du HTML selon le type d'email
            Dim htmlContent As String = ""
            Select Case typeEmail
                Case TypeEmail.Info
                    htmlContent = GenererHtmlInfo(sujet, message, signature, pieceJointes)
                Case TypeEmail.Erreur
                    htmlContent = GenererHtmlErreur(sujet, message, signature, pieceJointes)
                Case TypeEmail.Urgence
                    htmlContent = GenererHtmlUrgence(sujet, message, signature, pieceJointes)
                Case TypeEmail.Succes
                    htmlContent = GenererHtmlSucces(sujet, message, signature, pieceJointes)
                Case TypeEmail.Alerte
                    htmlContent = GenererHtmlAlerte(sujet, message, signature, pieceJointes)
                Case TypeEmail.Avertissement
                    htmlContent = GenererHtmlAvertissement(sujet, message, signature, pieceJointes)
                Case TypeEmail.Notification
                    htmlContent = GenererHtmlNotification(sujet, message, signature, pieceJointes)
            End Select

            ' Configuration du message email
            Using mailMessage As New MailMessage()
                mailMessage.From = New MailAddress(_fromEmail, _fromName)
                mailMessage.To.Add(destinataire)
                mailMessage.Subject = sujet
                mailMessage.Body = htmlContent
                mailMessage.IsBodyHtml = True
                mailMessage.Priority = If(typeEmail = TypeEmail.Urgence OrElse typeEmail = TypeEmail.Alerte, MailPriority.High, MailPriority.Normal)

                ' Ajout des CC
                If cc IsNot Nothing AndAlso cc.Count > 0 Then
                    For Each ccEmail In cc
                        If Not String.IsNullOrWhiteSpace(ccEmail) Then
                            mailMessage.CC.Add(ccEmail)
                        End If
                    Next
                End If

                ' Ajout des BCC
                If cci IsNot Nothing AndAlso cci.Count > 0 Then
                    For Each bccEmail In cci
                        If Not String.IsNullOrWhiteSpace(bccEmail) Then
                            mailMessage.Bcc.Add(bccEmail)
                        End If
                    Next
                End If

                ' Ajout des pièces jointes
                If fichiersAttaches IsNot Nothing AndAlso fichiersAttaches.Count > 0 Then
                    For Each fichier In fichiersAttaches
                        Try
                            If File.Exists(fichier) Then
                                Dim attachment As New Attachment(fichier)
                                mailMessage.Attachments.Add(attachment)
                            End If
                        Catch ex As Exception
                            Console.WriteLine($"Erreur ajout pièce jointe {fichier}: {ex.Message}")
                        End Try
                    Next
                End If

                ' Configuration du client SMTP
                Using smtpClient As New SmtpClient(_smtpHost, _smtpPort)
                    smtpClient.Credentials = New NetworkCredential(_username, _password)
                    smtpClient.EnableSsl = _enableSsl
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network
                    smtpClient.Timeout = 30000 ' 30 secondes

                    ' Envoi asynchrone
                    Await Task.Run(Sub() smtpClient.Send(mailMessage))
                    Return True
                End Using
            End Using

        Catch ex As SmtpException
            Console.WriteLine($"Erreur SMTP lors de l'envoi: {ex.Message}")
            If ex.InnerException IsNot Nothing Then
                Console.WriteLine($"Détails: {ex.InnerException.Message}")
            End If
            Return False
        Catch ex As Exception
            Console.WriteLine($"Exception lors de l'envoi de l'email: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Génère le template HTML pour un email de type Info (Bleu)
    ''' </summary>
    Private Function GenererHtmlInfo(sujet As String, message As String, signature As String, pieceJointes As List(Of String)) As String
        Return GenererTemplateBase(sujet, message, signature, pieceJointes, "#2196F3", "ℹ️", "MBTI Consult")
    End Function

    ''' <summary>
    ''' Génère le template HTML pour un email de type Erreur (Rouge)
    ''' </summary>
    Private Function GenererHtmlErreur(sujet As String, message As String, signature As String, pieceJointes As List(Of String)) As String
        Return GenererTemplateBase(sujet, message, signature, pieceJointes, "#f44336", "❌", "MBTI Consult")
    End Function

    ''' <summary>
    ''' Génère le template HTML pour un email de type Urgence (Orange)
    ''' </summary>
    Private Function GenererHtmlUrgence(sujet As String, message As String, signature As String, pieceJointes As List(Of String)) As String
        Return GenererTemplateBase(sujet, message, signature, pieceJointes, "#ff9800", "⚡", "MBTI Consult")
    End Function

    ''' <summary>
    ''' Génère le template HTML pour un email de type Succès (Vert)
    ''' </summary>
    Private Function GenererHtmlSucces(sujet As String, message As String, signature As String, pieceJointes As List(Of String)) As String
        Return GenererTemplateBase(sujet, message, signature, pieceJointes, "#4caf50", "✅", "MBTI Consult")
    End Function

    ''' <summary>
    ''' Génère le template HTML pour un email de type Alerte (Rouge foncé)
    ''' </summary>
    Private Function GenererHtmlAlerte(sujet As String, message As String, signature As String, pieceJointes As List(Of String)) As String
        Return GenererTemplateBase(sujet, message, signature, pieceJointes, "#b71c1c", "🚨", "MBTI Consult")
    End Function

    ''' <summary>
    ''' Génère le template HTML pour un email de type Avertissement (Jaune)
    ''' </summary>
    Private Function GenererHtmlAvertissement(sujet As String, message As String, signature As String, pieceJointes As List(Of String)) As String
        Return GenererTemplateBase(sujet, message, signature, pieceJointes, "#f57f17", "⚠️", "MBTI Consult")
    End Function

    ''' <summary>
    ''' Génère le template HTML pour un email de type Notification (Violet)
    ''' </summary>
    Private Function GenererHtmlNotification(sujet As String, message As String, signature As String, pieceJointes As List(Of String)) As String
        Return GenererTemplateBase(sujet, message, signature, pieceJointes, "#7b1fa2", "🔔", "MBTI Consult")
    End Function

    ''' <summary>
    ''' Génère le template HTML de base pour tous les types d'emails
    ''' </summary>
    Private Function GenererTemplateBase(sujet As String, message As String, signature As String, 
                                        pieceJointes As List(Of String), couleur As String, 
                                        icone As String, entreprise As String) As String
        Dim piecesJointesHtml As String = ""
        If pieceJointes IsNot Nothing AndAlso pieceJointes.Count > 0 Then
            Dim couleurClair = AjusterCouleur(couleur, 0.9)
            piecesJointesHtml = $"<div style='margin-top: 20px; padding: 15px; background-color: {couleurClair}; border-left: 4px solid {couleur}; border-radius: 4px;'>"
            piecesJointesHtml &= $"<p style='margin: 0 0 10px 0; font-weight: bold; color: {couleur};'>📎 Pièces jointes:</p>"
            piecesJointesHtml &= "<ul style='margin: 0; padding-left: 20px;'>"
            For Each piece In pieceJointes
                piecesJointesHtml &= $"<li style='color: #424242; margin: 5px 0;'>{piece}</li>"
            Next
            piecesJointesHtml &= "</ul></div>"
        End If

        Dim signatureHtml As String = ""
        If Not String.IsNullOrWhiteSpace(signature) Then
            signatureHtml = $"<div style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #e0e0e0; color: #757575; font-size: 14px;'>{signature.Replace(vbCrLf, "<br>").Replace(vbLf, "<br>")}</div>"
        End If

        Return $"<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f5f5f5;'>
    <div style='max-width: 600px; margin: 20px auto; background-color: white; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>
        <div style='background-color: {couleur}; padding: 30px; text-align: center;'>
            <h1 style='color: #ffffff !important; margin: 0; font-size: 24px; font-weight: 600;'>{icone} {sujet}</h1>
        </div>
        <div style='padding: 30px;'>
            <div style='color: #212121; font-size: 16px; line-height: 1.6;'>
                {message}
            </div>
            {piecesJointesHtml}
            {signatureHtml}
        </div>
        <div style='background-color: #f5f5f5; padding: 20px; text-align: center; border-top: 1px solid #e0e0e0;'>
            <p style='margin: 0; color: #9e9e9e; font-size: 12px;'>
                Cet email a été envoyé par <strong style='color: {couleur};'>{entreprise}</strong>
            </p>
        </div>
    </div>
</body>
</html>"
    End Function

    ''' <summary>
    ''' Ajuste la luminosité d'une couleur hexadécimale
    ''' </summary>
    Private Function AjusterCouleur(hexColor As String, facteur As Double) As String
        Try
            Dim r = Convert.ToInt32(hexColor.Substring(1, 2), 16)
            Dim g = Convert.ToInt32(hexColor.Substring(3, 2), 16)
            Dim b = Convert.ToInt32(hexColor.Substring(5, 2), 16)
            
            r = Math.Min(255, CInt(r + (255 - r) * facteur))
            g = Math.Min(255, CInt(g + (255 - g) * facteur))
            b = Math.Min(255, CInt(b + (255 - b) * facteur))
            
            Return $"#{r:X2}{g:X2}{b:X2}"
        Catch
            Return "#f5f5f5"
        End Try
    End Function
End Class
