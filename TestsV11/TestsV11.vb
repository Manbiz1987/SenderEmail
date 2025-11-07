Imports System
Imports System.Collections.Generic
Imports EmailSenderDLL

Module TestsV11
    ' Configuration
    Private Const API_KEY As String = "***REMOVED***"
    Private Const FROM_EMAIL As String = "***REMOVED***"
    Private Const FROM_NAME As String = "Tech Dev DAAM"
    Private Const TO_EMAIL As String = "***REMOVED***"

    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        Console.WriteLine("═══════════════════════════════════════════════════════════")
        Console.WriteLine("  TEST NOUVELLES FONCTIONNALITES - EmailSenderDLL v1.1.0")
        Console.WriteLine("═══════════════════════════════════════════════════════════")
        Console.WriteLine()

        ' Création de l'instance EmailSender
        Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

        ' Test 1 : Email de type Succes
        Console.WriteLine("📧 Test 1 : Email de type Succes")
        Console.WriteLine("------------------------------------------")
        TestEmailSucces(sender).Wait()
        Console.WriteLine()
        System.Threading.Thread.Sleep(2000) ' Pause 2 secondes

        ' Test 2 : Email avec pièce jointe
        Console.WriteLine("📧 Test 2 : Email avec pièce jointe réelle")
        Console.WriteLine("------------------------------------------")
        TestEmailAvecPieceJointe(sender).Wait()
        Console.WriteLine()
        System.Threading.Thread.Sleep(2000) ' Pause 2 secondes

        ' Test 3 : Email Succes avec pièce jointe
        Console.WriteLine("📧 Test 3 : Email Succes + Pièce jointe")
        Console.WriteLine("------------------------------------------")
        TestEmailSuccesAvecPieceJointe(sender).Wait()
        Console.WriteLine()

        Console.WriteLine("═══════════════════════════════════════════════════════════")
        Console.WriteLine("  TOUS LES TESTS SONT TERMINÉS !")
        Console.WriteLine("  Vérifiez votre boîte de réception : " & TO_EMAIL)
        Console.WriteLine("═══════════════════════════════════════════════════════════")
        Console.WriteLine()
        Console.WriteLine("Appuyez sur une touche pour quitter...")
        Console.ReadKey()
    End Sub

    Private Async Function TestEmailSucces(sender As EmailSender) As Task
        Try
            Dim sujet = "✅ Test Email Type Succes"
            Dim message = "Ceci est un email de test pour le nouveau type <strong>Succes</strong>. " &
                         "Ce type d'email utilise un template vert pour indiquer une opération réussie." &
                         vbCrLf & vbCrLf &
                         "🎉 Fonctionnalités du type Succes :" &
                         vbCrLf & "• Couleur verte (#4caf50)" &
                         vbCrLf & "• Icône ✅ dans l'en-tête" &
                         vbCrLf & "• Design moderne et responsive" &
                         vbCrLf & "• Adapté pour confirmer des actions positives"

            Dim signature = "Tech Dev DAAM" & vbCrLf &
                           "Développeur VB.NET" & vbCrLf &
                           "📧 ***REMOVED***"

            Console.Write("   Envoi en cours... ")
            Dim resultat = Await sender.EnvoyerEmailAsync(
                TO_EMAIL, sujet, message, TypeEmail.Succes, signature
            )

            If resultat Then
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine("✅ SUCCÈS")
                Console.ResetColor()
            Else
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("❌ ÉCHEC")
                Console.ResetColor()
            End If

        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("❌ ERREUR: " & ex.Message)
            Console.ResetColor()
        End Try
    End Function

    Private Async Function TestEmailAvecPieceJointe(sender As EmailSender) As Task
        Try
            Dim sujet = "📎 Test Email avec Pièce Jointe"
            Dim message = "Ceci est un email de test avec une <strong>pièce jointe réelle</strong>." &
                         vbCrLf & vbCrLf &
                         "Le fichier joint est encodé en Base64 et envoyé via l'API SendGrid." &
                         vbCrLf & vbCrLf &
                         "📄 Détails de la pièce jointe :" &
                         vbCrLf & "• Nom : fichier-test.txt" &
                         vbCrLf & "• Type : Fichier texte" &
                         vbCrLf & "• Encodage : Base64"

            Dim cheminFichier = "/Users/mahmoudbenelkhouja/Desktop/Tools/SenderEmail/fichier-test.txt"

            If Not System.IO.File.Exists(cheminFichier) Then
                Console.ForegroundColor = ConsoleColor.Yellow
                Console.WriteLine("⚠️  FICHIER INTROUVABLE: " & cheminFichier)
                Console.ResetColor()
                Return
            End If

            Console.WriteLine("   Fichier: " & System.IO.Path.GetFileName(cheminFichier))
            Console.Write("   Envoi en cours... ")

            Dim fichiers As New List(Of String) From {cheminFichier}

            Dim resultat = Await sender.EnvoyerEmailAsync(
                TO_EMAIL, sujet, message, TypeEmail.Info, Nothing, Nothing, fichiers
            )

            If resultat Then
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine("✅ SUCCÈS")
                Console.ResetColor()
            Else
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("❌ ÉCHEC")
                Console.ResetColor()
            End If

        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("❌ ERREUR: " & ex.Message)
            Console.ResetColor()
        End Try
    End Function

    Private Async Function TestEmailSuccesAvecPieceJointe(sender As EmailSender) As Task
        Try
            Dim sujet = "✅📎 Test Complet : Succes + Pièce Jointe"
            Dim message = "Ceci est un test <strong>combiné</strong> des deux nouvelles fonctionnalités :" &
                         vbCrLf & vbCrLf &
                         "✅ <strong>Nouveau type d'email : Succes</strong>" &
                         vbCrLf & "Email avec template vert pour les confirmations positives" &
                         vbCrLf & vbCrLf &
                         "📎 <strong>Pièce jointe réelle</strong>" &
                         vbCrLf & "Fichier attaché encodé en Base64"

            Dim signature = "🤖 Système Automatisé" & vbCrLf &
                           "EmailSenderDLL v1.1.0" & vbCrLf &
                           "Tech Dev DAAM"

            Dim cheminFichier = "/Users/mahmoudbenelkhouja/Desktop/Tools/SenderEmail/fichier-test.txt"

            If Not System.IO.File.Exists(cheminFichier) Then
                Console.ForegroundColor = ConsoleColor.Yellow
                Console.WriteLine("⚠️  FICHIER INTROUVABLE")
                Console.ResetColor()
                Return
            End If

            Console.WriteLine("   Fichier: " & System.IO.Path.GetFileName(cheminFichier))
            Console.Write("   Envoi en cours... ")

            Dim fichiers As New List(Of String) From {cheminFichier}

            Dim resultat = Await sender.EnvoyerEmailAsync(
                TO_EMAIL, sujet, message, TypeEmail.Succes, signature, Nothing, fichiers
            )

            If resultat Then
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine("✅ SUCCÈS")
                Console.ResetColor()
            Else
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("❌ ÉCHEC")
                Console.ResetColor()
            End If

        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("❌ ERREUR: " & ex.Message)
            Console.ResetColor()
        End Try
    End Function

End Module
