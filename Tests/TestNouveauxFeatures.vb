Imports System
Imports System.Collections.Generic
Imports EmailSenderDLL

Module TestNouvellesFonctionnalites
    ' Configuration
    Private Const API_KEY As String = "***REMOVED***"
    Private Const FROM_EMAIL As String = "***REMOVED***"
    Private Const FROM_NAME As String = "Tech Dev DAAM"
    Private Const TO_EMAIL As String = "***REMOVED***"

    Sub Main()
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

        ' Test 2 : Email avec pièce jointe
        Console.WriteLine("📧 Test 2 : Email avec pièce jointe réelle")
        Console.WriteLine("------------------------------------------")
        TestEmailAvecPieceJointe(sender).Wait()
        Console.WriteLine()

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

    ''' <summary>
    ''' Test du nouveau type d'email : Succes (Vert)
    ''' </summary>
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
                TO_EMAIL,
                sujet,
                message,
                TypeEmail.Succes,
                signature
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

    ''' <summary>
    ''' Test de l'envoi d'un email avec une pièce jointe réelle
    ''' </summary>
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
                         vbCrLf & "• Encodage : Base64" &
                         vbCrLf & vbCrLf &
                         "Vous devriez pouvoir télécharger et ouvrir ce fichier."

            ' Chemin du fichier de test
            Dim cheminFichier = System.IO.Path.Combine(
                System.IO.Directory.GetCurrentDirectory(),
                "..", "..", "..", "..",
                "fichier-test.txt"
            )

            ' Vérifier que le fichier existe
            If Not System.IO.File.Exists(cheminFichier) Then
                Console.ForegroundColor = ConsoleColor.Yellow
                Console.WriteLine("⚠️  FICHIER INTROUVABLE: " & cheminFichier)
                Console.ResetColor()
                Return
            End If

            Console.WriteLine("   Fichier trouvé: " & System.IO.Path.GetFileName(cheminFichier))
            Console.Write("   Envoi en cours... ")

            Dim fichiers As New List(Of String) From {cheminFichier}

            Dim resultat = Await sender.EnvoyerEmailAsync(
                TO_EMAIL,
                sujet,
                message,
                TypeEmail.Info,
                Nothing,
                Nothing,
                fichiers
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

    ''' <summary>
    ''' Test combiné : Email Succes avec pièce jointe
    ''' </summary>
    Private Async Function TestEmailSuccesAvecPieceJointe(sender As EmailSender) As Task
        Try
            Dim sujet = "✅📎 Test Complet : Succes + Pièce Jointe"
            Dim message = "Ceci est un test <strong>combiné</strong> des deux nouvelles fonctionnalités :" &
                         vbCrLf & vbCrLf &
                         "✅ <strong>Nouveau type d'email : Succes</strong>" &
                         vbCrLf & "Email avec template vert pour les confirmations positives" &
                         vbCrLf & vbCrLf &
                         "📎 <strong>Pièce jointe réelle</strong>" &
                         vbCrLf & "Fichier attaché encodé en Base64" &
                         vbCrLf & vbCrLf &
                         "Cette combinaison est idéale pour :" &
                         vbCrLf & "• Confirmer l'envoi d'un rapport" &
                         vbCrLf & "• Notifier la réussite d'une opération avec documents" &
                         vbCrLf & "• Envoyer des confirmations avec reçus"

            Dim signature = "🤖 Système Automatisé" & vbCrLf &
                           "EmailSenderDLL v1.1.0" & vbCrLf &
                           "Tech Dev DAAM"

            ' Chemin du fichier de test
            Dim cheminFichier = System.IO.Path.Combine(
                System.IO.Directory.GetCurrentDirectory(),
                "..", "..", "..", "..",
                "fichier-test.txt"
            )

            If Not System.IO.File.Exists(cheminFichier) Then
                Console.ForegroundColor = ConsoleColor.Yellow
                Console.WriteLine("⚠️  FICHIER INTROUVABLE: " & cheminFichier)
                Console.ResetColor()
                Return
            End If

            Console.WriteLine("   Fichier trouvé: " & System.IO.Path.GetFileName(cheminFichier))
            Console.Write("   Envoi en cours... ")

            Dim fichiers As New List(Of String) From {cheminFichier}

            Dim resultat = Await sender.EnvoyerEmailAsync(
                TO_EMAIL,
                sujet,
                message,
                TypeEmail.Succes,
                signature,
                Nothing,
                fichiers
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
