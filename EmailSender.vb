Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks
Imports Newtonsoft.Json

''' <summary>
''' Types d'emails disponibles avec templates HTML distincts
''' </summary>
Public Enum TypeEmail
    ''' <summary>Email informatif (Bleu)</summary>
    Info = 0
    ''' <summary>Email d'erreur (Rouge)</summary>
    Erreur = 1
    ''' <summary>Email urgent (Orange)</summary>
    Urgence = 2
    ''' <summary>Email de succès (Vert)</summary>
    Succes = 3
    ''' <summary>Email d'alerte (Rouge foncé)</summary>
    Alerte = 4
    ''' <summary>Email d'avertissement (Jaune)</summary>
    Avertissement = 5
    ''' <summary>Email de notification (Violet)</summary>
    Notification = 6
End Enum

''' <summary>
''' Classe pour envoyer des emails via SendGrid API
''' </summary>
Public Class EmailSender
    Private ReadOnly _apiKey As String
    Private ReadOnly _fromEmail As String
    Private ReadOnly _fromName As String
    Private Const SENDGRID_API_URL As String = "https://api.sendgrid.com/v3/mail/send"

    ''' <summary>
    ''' Initialise une nouvelle instance de EmailSender
    ''' </summary>
    ''' <param name="apiKey">Clé API SendGrid</param>
    ''' <param name="fromEmail">Adresse email de l'expéditeur</param>
    ''' <param name="fromName">Nom affiché de l'expéditeur</param>
    Public Sub New(apiKey As String, fromEmail As String, fromName As String)
        If String.IsNullOrWhiteSpace(apiKey) Then Throw New ArgumentException("La clé API ne peut pas être vide", NameOf(apiKey))
        If String.IsNullOrWhiteSpace(fromEmail) Then Throw New ArgumentException("L'email expéditeur ne peut pas être vide", NameOf(fromEmail))
        If String.IsNullOrWhiteSpace(fromName) Then Throw New ArgumentException("Le nom expéditeur ne peut pas être vide", NameOf(fromName))

        _apiKey = apiKey
        _fromEmail = fromEmail
        _fromName = fromName
    End Sub

    ''' <summary>
    ''' Envoie un email avec template personnalisé
    ''' </summary>
    ''' <param name="destinataire">Adresse email du destinataire principal</param>
    ''' <param name="sujet">Sujet de l'email</param>
    ''' <param name="message">Contenu du message (HTML autorisé)</param>
    ''' <param name="typeEmail">Type d'email (Info, Erreur, Urgence, Succes)</param>
    ''' <param name="signature">Signature personnalisée (optionnel)</param>
    ''' <param name="pieceJointes">Liste des noms de pièces jointes à afficher (optionnel)</param>
    ''' <param name="fichiersAttaches">Liste des chemins complets des fichiers à attacher (optionnel)</param>
    ''' <param name="cc">Liste des destinataires en copie (optionnel)</param>
    ''' <param name="cci">Liste des destinataires en copie cachée (optionnel)</param>
    ''' <returns>True si l'email est envoyé avec succès, False sinon</returns>
    Public Async Function EnvoyerEmailAsync(
        destinataire As String,
        sujet As String,
        message As String,
        Optional typeEmail As TypeEmail = TypeEmail.Info,
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

            ' Construction de la liste des destinataires
            Dim personalizations As New List(Of Object)()
            Dim toList As New List(Of Object) From {
                New With {.email = destinataire}
            }

            ' Ajout des CC si présents
            Dim ccList As List(Of Object) = Nothing
            If cc IsNot Nothing AndAlso cc.Count > 0 Then
                ccList = New List(Of Object)()
                For Each emailCc In cc
                    ccList.Add(New With {.email = emailCc})
                Next
            End If

            ' Ajout des CCI si présents
            Dim bccList As List(Of Object) = Nothing
            If cci IsNot Nothing AndAlso cci.Count > 0 Then
                bccList = New List(Of Object)()
                For Each emailCci In cci
                    bccList.Add(New With {.email = emailCci})
                Next
            End If

            ' Construction de l'objet personalizations
            Dim personalization As Object
            If ccList IsNot Nothing AndAlso bccList IsNot Nothing Then
                personalization = New With {
                    .to = toList,
                    .cc = ccList,
                    .bcc = bccList
                }
            ElseIf ccList IsNot Nothing Then
                personalization = New With {
                    .to = toList,
                    .cc = ccList
                }
            ElseIf bccList IsNot Nothing Then
                personalization = New With {
                    .to = toList,
                    .bcc = bccList
                }
            Else
                personalization = New With {
                    .to = toList
                }
            End If

            personalizations.Add(personalization)

            ' Gestion des pièces jointes (fichiers réels)
            Dim attachments As List(Of Object) = Nothing
            If fichiersAttaches IsNot Nothing AndAlso fichiersAttaches.Count > 0 Then
                attachments = New List(Of Object)()
                For Each fichier In fichiersAttaches
                    Try
                        If IO.File.Exists(fichier) Then
                            Dim fileBytes = IO.File.ReadAllBytes(fichier)
                            Dim base64Content = Convert.ToBase64String(fileBytes)
                            Dim fileName = IO.Path.GetFileName(fichier)
                            
                            attachments.Add(New With {
                                .content = base64Content,
                                .filename = fileName,
                                .type = "application/octet-stream",
                                .disposition = "attachment"
                            })
                        End If
                    Catch ex As Exception
                        Console.WriteLine($"Erreur lecture fichier {fichier}: {ex.Message}")
                    End Try
                Next
            End If

            ' Construction du payload JSON pour SendGrid
            Dim payload As Object
            If attachments IsNot Nothing AndAlso attachments.Count > 0 Then
                payload = New With {
                    .personalizations = personalizations,
                    .from = New With {
                        .email = _fromEmail,
                        .name = _fromName
                    },
                    .subject = sujet,
                    .content = New Object() {
                        New With {
                            .type = "text/html",
                            .value = htmlContent
                        }
                    },
                    .attachments = attachments
                }
            Else
                payload = New With {
                    .personalizations = personalizations,
                    .from = New With {
                        .email = _fromEmail,
                        .name = _fromName
                    },
                    .subject = sujet,
                    .content = New Object() {
                        New With {
                            .type = "text/html",
                            .value = htmlContent
                        }
                    }
                }
            End If

            Dim jsonPayload As String = JsonConvert.SerializeObject(payload)

            ' Envoi de la requête à SendGrid
            Using client As New HttpClient()
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " & _apiKey)
                
                Dim content As New StringContent(jsonPayload, Encoding.UTF8, "application/json")
                Dim response = Await client.PostAsync(SENDGRID_API_URL, content)

                If response.IsSuccessStatusCode Then
                    Return True
                Else
                    Dim errorContent = Await response.Content.ReadAsStringAsync()
                    Console.WriteLine($"Erreur SendGrid [{response.StatusCode}]: {errorContent}")
                    Return False
                End If
            End Using

        Catch ex As Exception
            Console.WriteLine($"Exception lors de l'envoi de l'email: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Génère le template HTML pour un email de type Info (Bleu)
    ''' </summary>
    Private Function GenererHtmlInfo(sujet As String, message As String, signature As String, pieceJointes As List(Of String)) As String
        Dim piecesJointesHtml As String = ""
        If pieceJointes IsNot Nothing AndAlso pieceJointes.Count > 0 Then
            piecesJointesHtml = "<div style='margin-top: 20px; padding: 15px; background-color: #e3f2fd; border-left: 4px solid #2196F3; border-radius: 4px;'>"
            piecesJointesHtml &= "<p style='margin: 0 0 10px 0; font-weight: bold; color: #1976D2;'>📎 Pièces jointes:</p>"
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
        <!-- Header Bleu -->
        <div style='background-color: #2196F3; padding: 30px; text-align: center;'>
            <h1 style='color: #ffffff !important; margin: 0; font-size: 24px; font-weight: 600;'>ℹ️ {sujet}</h1>
        </div>
        
        <!-- Contenu -->
        <div style='padding: 30px;'>
            <div style='color: #212121; font-size: 16px; line-height: 1.6;'>
                {message}
            </div>
            
            {piecesJointesHtml}
            {signatureHtml}
        </div>
        
        <!-- Footer -->
        <div style='background-color: #f5f5f5; padding: 20px; text-align: center; border-top: 1px solid #e0e0e0;'>
            <p style='margin: 0; color: #9e9e9e; font-size: 12px;'>
                Cet email a été envoyé par <strong style='color: #2196F3;'>Tech Dev DAAM</strong>
            </p>
        </div>
    </div>
</body>
</html>"
    End Function

    ''' <summary>
    ''' Génère le template HTML pour un email de type Erreur (Rouge)
    ''' </summary>
    Private Function GenererHtmlErreur(sujet As String, message As String, signature As String, pieceJointes As List(Of String)) As String
        Dim piecesJointesHtml As String = ""
        If pieceJointes IsNot Nothing AndAlso pieceJointes.Count > 0 Then
            piecesJointesHtml = "<div style='margin-top: 20px; padding: 15px; background-color: #ffebee; border-left: 4px solid #f44336; border-radius: 4px;'>"
            piecesJointesHtml &= "<p style='margin: 0 0 10px 0; font-weight: bold; color: #c62828;'>📎 Pièces jointes:</p>"
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
        <!-- Header Rouge -->
        <div style='background-color: #f44336; padding: 30px; text-align: center;'>
            <h1 style='color: #ffffff !important; margin: 0; font-size: 24px; font-weight: 600;'>❌ {sujet}</h1>
        </div>
        
        <!-- Contenu -->
        <div style='padding: 30px;'>
            <div style='color: #212121; font-size: 16px; line-height: 1.6;'>
                {message}
            </div>
            
            {piecesJointesHtml}
            {signatureHtml}
        </div>
        
        <!-- Footer -->
        <div style='background-color: #f5f5f5; padding: 20px; text-align: center; border-top: 1px solid #e0e0e0;'>
            <p style='margin: 0; color: #9e9e9e; font-size: 12px;'>
                Cet email a été envoyé par <strong style='color: #f44336;'>Tech Dev DAAM</strong>
            </p>
        </div>
    </div>
</body>
</html>"
    End Function

    ''' <summary>
    ''' Génère le template HTML pour un email de type Urgence (Orange)
    ''' </summary>
    Private Function GenererHtmlUrgence(sujet As String, message As String, signature As String, pieceJointes As List(Of String)) As String
        Dim piecesJointesHtml As String = ""
        If pieceJointes IsNot Nothing AndAlso pieceJointes.Count > 0 Then
            piecesJointesHtml = "<div style='margin-top: 20px; padding: 15px; background-color: #fff3e0; border-left: 4px solid #ff9800; border-radius: 4px;'>"
            piecesJointesHtml &= "<p style='margin: 0 0 10px 0; font-weight: bold; color: #e65100;'>📎 Pièces jointes:</p>"
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
        <!-- Header Orange -->
        <div style='background-color: #ff9800; padding: 30px; text-align: center;'>
            <h1 style='color: #ffffff !important; margin: 0; font-size: 24px; font-weight: 600;'>⚡ {sujet}</h1>
        </div>
        
        <!-- Contenu -->
        <div style='padding: 30px;'>
            <div style='color: #212121; font-size: 16px; line-height: 1.6;'>
                {message}
            </div>
            
            {piecesJointesHtml}
            {signatureHtml}
        </div>
        
        <!-- Footer -->
        <div style='background-color: #f5f5f5; padding: 20px; text-align: center; border-top: 1px solid #e0e0e0;'>
            <p style='margin: 0; color: #9e9e9e; font-size: 12px;'>
                Cet email a été envoyé par <strong style='color: #ff9800;'>Tech Dev DAAM</strong>
            </p>
        </div>
    </div>
</body>
</html>"
    End Function

    ''' <summary>
    ''' Génère le template HTML pour un email de type Succès (Vert)
    ''' </summary>
    Private Function GenererHtmlSucces(sujet As String, message As String, signature As String, pieceJointes As List(Of String)) As String
        Dim piecesJointesHtml As String = ""
        If pieceJointes IsNot Nothing AndAlso pieceJointes.Count > 0 Then
            piecesJointesHtml = "<div style='margin-top: 20px; padding: 15px; background-color: #e8f5e9; border-left: 4px solid #4caf50; border-radius: 4px;'>"
            piecesJointesHtml &= "<p style='margin: 0 0 10px 0; font-weight: bold; color: #2e7d32;'>📎 Pièces jointes:</p>"
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
        <!-- Header Vert -->
        <div style='background-color: #4caf50; padding: 30px; text-align: center;'>
            <h1 style='color: #ffffff !important; margin: 0; font-size: 24px; font-weight: 600;'>✅ {sujet}</h1>
        </div>
        
        <!-- Contenu -->
        <div style='padding: 30px;'>
            <div style='color: #212121; font-size: 16px; line-height: 1.6;'>
                {message}
            </div>
            
            {piecesJointesHtml}
            {signatureHtml}
        </div>
        
        <!-- Footer -->
        <div style='background-color: #f5f5f5; padding: 20px; text-align: center; border-top: 1px solid #e0e0e0;'>
            <p style='margin: 0; color: #9e9e9e; font-size: 12px;'>
                Cet email a été envoyé par <strong style='color: #4caf50;'>Tech Dev DAAM</strong>
            </p>
        </div>
    </div>
</body>
</html>"
    End Function

    ''' <summary>
    ''' Génère le HTML pour un email de type Alerte (Rouge foncé)
    ''' </summary>
    Private Function GenererHtmlAlerte(sujet As String, message As String, signature As String, pieceJointes As List(Of String)) As String
        Dim piecesJointesHtml As String = ""
        If pieceJointes IsNot Nothing AndAlso pieceJointes.Count > 0 Then
            piecesJointesHtml = "<div style='margin-top: 20px; padding: 15px; background-color: #ffebee; border-left: 4px solid #b71c1c; border-radius: 4px;'>"
            piecesJointesHtml &= "<p style='margin: 0 0 10px 0; font-weight: bold; color: #b71c1c;'>📎 Pièces jointes :</p>"
            piecesJointesHtml &= "<ul style='margin: 0; padding-left: 20px;'>"
            For Each piece In pieceJointes
                piecesJointesHtml &= $"<li style='color: #666;'>{piece}</li>"
            Next
            piecesJointesHtml &= "</ul></div>"
        End If

        Dim signatureHtml As String = ""
        If Not String.IsNullOrWhiteSpace(signature) Then
            signatureHtml = $"<div style='margin-top: 30px; padding-top: 20px; border-top: 2px solid #ffebee;'>
                <p style='margin: 0; color: #666; font-size: 14px; white-space: pre-line;'>{signature}</p>
            </div>"
        End If

        Return $"<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f5f5f5;'>
    <div style='max-width: 600px; margin: 20px auto; background-color: white; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>
        <div style='background-color: #b71c1c; padding: 30px; text-align: center;'>
            <h1 style='margin: 0; color: #ffffff !important; font-size: 24px;'>🚨 {sujet}</h1>
        </div>
        <div style='padding: 30px;'>
            <div style='color: #333; font-size: 16px; line-height: 1.6; white-space: pre-line;'>
                {message}
            </div>
            {piecesJointesHtml}
            {signatureHtml}
        </div>
        <div style='background-color: #f5f5f5; padding: 20px; text-align: center; border-top: 1px solid #e0e0e0;'>
            <p style='margin: 0; color: #9e9e9e; font-size: 12px;'>
                Cet email a été envoyé par <strong style='color: #b71c1c;'>Tech Dev DAAM</strong>
            </p>
        </div>
    </div>
</body>
</html>"
    End Function

    ''' <summary>
    ''' Génère le HTML pour un email de type Avertissement (Jaune)
    ''' </summary>
    Private Function GenererHtmlAvertissement(sujet As String, message As String, signature As String, pieceJointes As List(Of String)) As String
        Dim piecesJointesHtml As String = ""
        If pieceJointes IsNot Nothing AndAlso pieceJointes.Count > 0 Then
            piecesJointesHtml = "<div style='margin-top: 20px; padding: 15px; background-color: #fffde7; border-left: 4px solid #f57f17; border-radius: 4px;'>"
            piecesJointesHtml &= "<p style='margin: 0 0 10px 0; font-weight: bold; color: #f57f17;'>📎 Pièces jointes :</p>"
            piecesJointesHtml &= "<ul style='margin: 0; padding-left: 20px;'>"
            For Each piece In pieceJointes
                piecesJointesHtml &= $"<li style='color: #666;'>{piece}</li>"
            Next
            piecesJointesHtml &= "</ul></div>"
        End If

        Dim signatureHtml As String = ""
        If Not String.IsNullOrWhiteSpace(signature) Then
            signatureHtml = $"<div style='margin-top: 30px; padding-top: 20px; border-top: 2px solid #fffde7;'>
                <p style='margin: 0; color: #666; font-size: 14px; white-space: pre-line;'>{signature}</p>
            </div>"
        End If

        Return $"<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f5f5f5;'>
    <div style='max-width: 600px; margin: 20px auto; background-color: white; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>
        <div style='background-color: #f57f17; padding: 30px; text-align: center;'>
            <h1 style='margin: 0; color: #ffffff !important; font-size: 24px;'>⚠️ {sujet}</h1>
        </div>
        <div style='padding: 30px;'>
            <div style='color: #333; font-size: 16px; line-height: 1.6; white-space: pre-line;'>
                {message}
            </div>
            {piecesJointesHtml}
            {signatureHtml}
        </div>
        <div style='background-color: #f5f5f5; padding: 20px; text-align: center; border-top: 1px solid #e0e0e0;'>
            <p style='margin: 0; color: #9e9e9e; font-size: 12px;'>
                Cet email a été envoyé par <strong style='color: #f57f17;'>Tech Dev DAAM</strong>
            </p>
        </div>
    </div>
</body>
</html>"
    End Function

    ''' <summary>
    ''' Génère le HTML pour un email de type Notification (Violet)
    ''' </summary>
    Private Function GenererHtmlNotification(sujet As String, message As String, signature As String, pieceJointes As List(Of String)) As String
        Dim piecesJointesHtml As String = ""
        If pieceJointes IsNot Nothing AndAlso pieceJointes.Count > 0 Then
            piecesJointesHtml = "<div style='margin-top: 20px; padding: 15px; background-color: #f3e5f5; border-left: 4px solid #7b1fa2; border-radius: 4px;'>"
            piecesJointesHtml &= "<p style='margin: 0 0 10px 0; font-weight: bold; color: #7b1fa2;'>📎 Pièces jointes :</p>"
            piecesJointesHtml &= "<ul style='margin: 0; padding-left: 20px;'>"
            For Each piece In pieceJointes
                piecesJointesHtml &= $"<li style='color: #666;'>{piece}</li>"
            Next
            piecesJointesHtml &= "</ul></div>"
        End If

        Dim signatureHtml As String = ""
        If Not String.IsNullOrWhiteSpace(signature) Then
            signatureHtml = $"<div style='margin-top: 30px; padding-top: 20px; border-top: 2px solid #f3e5f5;'>
                <p style='margin: 0; color: #666; font-size: 14px; white-space: pre-line;'>{signature}</p>
            </div>"
        End If

        Return $"<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f5f5f5;'>
    <div style='max-width: 600px; margin: 20px auto; background-color: white; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>
        <div style='background-color: #7b1fa2; padding: 30px; text-align: center;'>
            <h1 style='margin: 0; color: #ffffff !important; font-size: 24px;'>🔔 {sujet}</h1>
        </div>
        <div style='padding: 30px;'>
            <div style='color: #333; font-size: 16px; line-height: 1.6; white-space: pre-line;'>
                {message}
            </div>
            {piecesJointesHtml}
            {signatureHtml}
        </div>
        <div style='background-color: #f5f5f5; padding: 20px; text-align: center; border-top: 1px solid #e0e0e0;'>
            <p style='margin: 0; color: #9e9e9e; font-size: 12px;'>
                Cet email a été envoyé par <strong style='color: #7b1fa2;'>Tech Dev DAAM</strong>
            </p>
        </div>
    </div>
</body>
</html>"
    End Function
End Class
