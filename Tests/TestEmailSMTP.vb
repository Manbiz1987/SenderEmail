Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Threading.Tasks

Module TestEmailSMTP
    Sub Main()
        Console.WriteLine("===================================")
        Console.WriteLine("TEST EmailSenderSMTP - Gmail SMTP")
        Console.WriteLine("===================================")
        Console.WriteLine()

        Try
            ' Chargement des variables d'environnement
            EnvConfig.LoadEnvFile()
            
            Dim smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST")
            Dim smtpPort = Integer.Parse(Environment.GetEnvironmentVariable("SMTP_PORT"))
            Dim smtpUser = Environment.GetEnvironmentVariable("SMTP_USERNAME")
            Dim smtpPass = Environment.GetEnvironmentVariable("SMTP_PASSWORD")
            Dim fromEmail = Environment.GetEnvironmentVariable("SMTP_FROM_EMAIL")
            Dim fromName = Environment.GetEnvironmentVariable("SMTP_FROM_NAME")
            Dim toEmail = Environment.GetEnvironmentVariable("TEST_TO_EMAIL")

            Console.WriteLine($"Configuration SMTP:")
            Console.WriteLine($"  - Host: {smtpHost}")
            Console.WriteLine($"  - Port: {smtpPort}")
            Console.WriteLine($"  - From: {fromName} <{fromEmail}>")
            Console.WriteLine($"  - To: {toEmail}")
            Console.WriteLine()

            ' Création du sender SMTP
            Dim sender As New EmailSenderSMTP(
                smtpHost:=smtpHost,
                smtpPort:=smtpPort,
                username:=smtpUser,
                password:=smtpPass,
                fromEmail:=fromEmail,
                fromName:=fromName,
                enableSsl:=True
            )

            ' Test 1: Email Info
            Console.WriteLine("Test 1: Envoi d'un email Info via SMTP...")
            Dim resultat1 = sender.EnvoyerEmailAsync(
                destinataire:=toEmail,
                sujet:="Test SMTP - Email Info",
                message:="Ceci est un email de test envoyé via <strong>Gmail SMTP</strong>.<br><br>La nouvelle méthode EmailSenderSMTP fonctionne correctement !",
                typeEmail:=TypeEmail.Info,
                signature:="Cordialement,<br>MBTI Consult<br>Email: mbticonsult@gmail.com"
            ).Result

            If resultat1 Then
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine("✓ Email Info envoyé avec succès via SMTP !")
                Console.ResetColor()
            Else
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("✗ Échec de l'envoi de l'email Info")
                Console.ResetColor()
            End If

            Console.WriteLine()
            System.Threading.Thread.Sleep(2000) ' Pause de 2 secondes

            ' Test 2: Email Succès
            Console.WriteLine("Test 2: Envoi d'un email Succès via SMTP...")
            Dim resultat2 = sender.EnvoyerEmailAsync(
                destinataire:=toEmail,
                sujet:="Test SMTP - Email Succès",
                message:="<strong>Félicitations !</strong><br><br>L'intégration SMTP Gmail est opérationnelle.<br>Vous pouvez maintenant utiliser les deux méthodes :<br>• EmailSender (Resend API)<br>• EmailSenderSMTP (SMTP)",
                typeEmail:=TypeEmail.Succes,
                signature:="Équipe Technique<br>MBTI Consult"
            ).Result

            If resultat2 Then
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine("✓ Email Succès envoyé avec succès via SMTP !")
                Console.ResetColor()
            Else
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("✗ Échec de l'envoi de l'email Succès")
                Console.ResetColor()
            End If

            Console.WriteLine()
            System.Threading.Thread.Sleep(2000)

            ' Test 3: Email Urgence
            Console.WriteLine("Test 3: Envoi d'un email Urgence via SMTP...")
            Dim resultat3 = sender.EnvoyerEmailAsync(
                destinataire:=toEmail,
                sujet:="Test SMTP - Email Urgence",
                message:="<strong>ACTION REQUISE</strong><br><br>Ceci est un test d'email urgent envoyé via SMTP Gmail.<br><br>La DLL EmailSenderDLL est maintenant flexible avec 2 options d'envoi !",
                typeEmail:=TypeEmail.Urgence,
                signature:="MBTI Consult<br>Support Technique"
            ).Result

            If resultat3 Then
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine("✓ Email Urgence envoyé avec succès via SMTP !")
                Console.ResetColor()
            Else
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("✗ Échec de l'envoi de l'email Urgence")
                Console.ResetColor()
            End If

            Console.WriteLine()
            Console.WriteLine("===================================")
            Console.WriteLine("TESTS TERMINÉS")
            Console.WriteLine("===================================")
            Console.WriteLine()
            Console.WriteLine("Vérifiez votre boîte email: " & toEmail)
            Console.WriteLine()

        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine($"ERREUR: {ex.Message}")
            Console.WriteLine($"Type: {ex.GetType().Name}")
            If ex.InnerException IsNot Nothing Then
                Console.WriteLine($"Détails: {ex.InnerException.Message}")
            End If
            Console.ResetColor()
        End Try

        Console.WriteLine("Appuyez sur une touche pour quitter...")
        Console.ReadKey()
    End Sub
End Module
