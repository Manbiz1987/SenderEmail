Imports System
Imports EmailSenderDLL

Module TestAvecPieceJointe
    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        Console.WriteLine("=========================================================")
        Console.WriteLine("TEST ENVOI EMAIL AVEC PIÈCE JOINTE PDF")
        Console.WriteLine("=========================================================")
        Console.WriteLine()

        Try
            ' Configuration automatique depuis .env
            Dim sender As New EmailSenderSMTP()
            
            Dim destinataire = Environment.GetEnvironmentVariable("TEST_TO_EMAIL")
            
            ' Chemin absolu du fichier PDF
            Dim cheminPDF = "/Users/mahmoudbenelkhouja/Desktop/Tools/SenderEmail/Rapport_Anomalies_ACM_20260107_144132.pdf"
            
            Console.WriteLine($"📧 Destinataire : {destinataire}")
            Console.WriteLine($"📎 Pièce jointe : {IO.Path.GetFileName(cheminPDF)}")
            Console.WriteLine()
            
            ' Vérification que le fichier existe
            If Not IO.File.Exists(cheminPDF) Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine($"❌ ERREUR : Le fichier n'existe pas : {cheminPDF}")
                Console.ResetColor()
                Console.WriteLine("Appuyez sur une touche pour quitter...")
                Console.ReadKey()
                Return
            End If
            
            ' Récupération de la taille du fichier
            Dim tailleFichier = New IO.FileInfo(cheminPDF).Length
            Dim tailleMB = Math.Round(tailleFichier / 1024.0 / 1024.0, 2)
            
            Console.WriteLine($"📊 Taille du fichier : {tailleMB} MB")
            Console.WriteLine()
            Console.WriteLine("📤 Envoi en cours...")
            Console.WriteLine()
            
            ' Envoi de l'email avec la pièce jointe
            Dim resultat = sender.EnvoyerEmailAsync(
                message:="<h2>📄 Rapport d'Anomalies ACM</h2>" &
                        "<p>Bonjour,</p>" &
                        "<p>Veuillez trouver ci-joint le <strong>rapport d'anomalies ACM</strong> du 07/01/2026.</p>" &
                        "<h3>Détails du rapport :</h3>" &
                        "<ul>" &
                        $"<li>📅 Date : 07 Janvier 2026</li>" &
                        $"<li>🕐 Heure : 14:41:32</li>" &
                        $"<li>📦 Type : Rapport d'anomalies</li>" &
                        $"<li>📊 Taille : {tailleMB} MB</li>" &
                        "</ul>" &
                        "<p>Ce rapport contient l'analyse complète des anomalies détectées.</p>" &
                        "<p>Merci de votre attention.</p>",
                destinataire:=destinataire,
                sujet:="📄 Rapport d'Anomalies ACM - 07/01/2026",
                typeEmail:=TypeEmail.Info,
                signature:="Cordialement,<br><strong>MBTI Consult</strong><br>Système de Reporting Automatique<br>Email: mbticonsult@gmail.com",
                fichiersAttaches:=New List(Of String) From {cheminPDF}
            ).Result

            Console.WriteLine()
            
            If resultat Then
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine("✅ EMAIL ENVOYÉ AVEC SUCCÈS !")
                Console.WriteLine()
                Console.WriteLine("📋 Récapitulatif :")
                Console.WriteLine($"   ✓ Destinataire : {destinataire}")
                Console.WriteLine($"   ✓ Sujet : Rapport d'Anomalies ACM - 07/01/2026")
                Console.WriteLine($"   ✓ Type : Info (Bleu)")
                Console.WriteLine($"   ✓ Pièce jointe : {IO.Path.GetFileName(cheminPDF)} ({tailleMB} MB)")
                Console.WriteLine($"   ✓ Template HTML : Responsive avec signature")
                Console.ResetColor()
                Console.WriteLine()
                Console.WriteLine("🎉 Le fichier PDF a été attaché et envoyé avec succès !")
            Else
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("❌ ÉCHEC DE L'ENVOI")
                Console.WriteLine()
                Console.WriteLine("Vérifiez :")
                Console.WriteLine("  - La configuration SMTP dans le fichier .env")
                Console.WriteLine("  - Votre connexion internet")
                Console.WriteLine("  - Le mot de passe d'application Gmail")
                Console.ResetColor()
            End If

        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine()
            Console.WriteLine($"❌ ERREUR : {ex.Message}")
            Console.WriteLine()
            Console.WriteLine("Détails :")
            Console.WriteLine(ex.StackTrace)
            Console.ResetColor()
        End Try

        Console.WriteLine()
        Console.WriteLine("=========================================================")
        Console.WriteLine("Appuyez sur une touche pour quitter...")
        Console.ReadKey()
    End Sub
End Module
