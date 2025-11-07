Imports System
Imports System.Collections.Generic
Imports EmailSenderDLL

Module TestsV12
    Private Const API_KEY As String = "***REMOVED***"
    Private Const FROM_EMAIL As String = "***REMOVED***"
    Private Const FROM_NAME As String = "Tech Dev DAAM"
    Private Const TO_EMAIL As String = "***REMOVED***"

    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        Console.WriteLine("═══════════════════════════════════════════════════════════")
        Console.WriteLine("  TEST 3 NOUVEAUX TYPES - EmailSenderDLL v1.2.0")
        Console.WriteLine("═══════════════════════════════════════════════════════════")
        Console.WriteLine()

        Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

        Console.WriteLine("📧 Test 1 : Email type ALERTE (Rouge foncé)")
        Console.WriteLine("------------------------------------------")
        TestEmailAlerte(sender).Wait()
        Console.WriteLine()
        System.Threading.Thread.Sleep(2000)

        Console.WriteLine("📧 Test 2 : Email type AVERTISSEMENT (Jaune)")
        Console.WriteLine("------------------------------------------")
        TestEmailAvertissement(sender).Wait()
        Console.WriteLine()
        System.Threading.Thread.Sleep(2000)

        Console.WriteLine("📧 Test 3 : Email type NOTIFICATION (Violet)")
        Console.WriteLine("------------------------------------------")
        TestEmailNotification(sender).Wait()
        Console.WriteLine()
        System.Threading.Thread.Sleep(2000)

        Console.WriteLine("📧 Test 4 : Test COMPLET - Tous les 7 types")
        Console.WriteLine("------------------------------------------")
        TestTousLesTypes(sender).Wait()
        Console.WriteLine()

        Console.WriteLine("═══════════════════════════════════════════════════════════")
        Console.WriteLine("  TOUS LES TESTS SONT TERMINÉS !")
        Console.WriteLine("  Total types d'emails : 7")
        Console.WriteLine("  Vérifiez : " & TO_EMAIL)
        Console.WriteLine("═══════════════════════════════════════════════════════════")
        Console.WriteLine()
        Console.WriteLine("Appuyez sur une touche pour quitter...")
        Console.ReadKey()
    End Sub

    Private Async Function TestEmailAlerte(sender As EmailSender) As Task
        Try
            Dim sujet = "🚨 ALERTE SYSTÈME CRITIQUE"
            Dim message = "Une <strong>alerte critique</strong> a été détectée dans le système." &
                         vbCrLf & vbCrLf &
                         "⚠️ Détails de l'alerte :" &
                         vbCrLf & "• Type : Rouge foncé (#b71c1c)" &
                         vbCrLf & "• Icône : 🚨" &
                         vbCrLf & "• Gradient : #b71c1c → #d32f2f" &
                         vbCrLf & vbCrLf &
                         "Ce type est idéal pour :" &
                         vbCrLf & "• Incidents de sécurité" &
                         vbCrLf & "• Pannes système critiques" &
                         vbCrLf & "• Violations de politique"

            Console.Write("   Envoi en cours... ")
            Dim resultat = Await sender.EnvoyerEmailAsync(TO_EMAIL, sujet, message, TypeEmail.Alerte)

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

    Private Async Function TestEmailAvertissement(sender As EmailSender) As Task
        Try
            Dim sujet = "⚠️ Avertissement Important"
            Dim message = "Ceci est un <strong>avertissement</strong> nécessitant votre attention." &
                         vbCrLf & vbCrLf &
                         "📋 Caractéristiques :" &
                         vbCrLf & "• Couleur : Jaune (#f57f17)" &
                         vbCrLf & "• Icône : ⚠️" &
                         vbCrLf & "• Gradient : #f57f17 → #fbc02d" &
                         vbCrLf & vbCrLf &
                         "Utilisations recommandées :" &
                         vbCrLf & "• Avertissements de maintenance" &
                         vbCrLf & "• Dépassements de seuils" &
                         vbCrLf & "• Actions requises non urgentes"

            Console.Write("   Envoi en cours... ")
            Dim resultat = Await sender.EnvoyerEmailAsync(TO_EMAIL, sujet, message, TypeEmail.Avertissement)

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

    Private Async Function TestEmailNotification(sender As EmailSender) As Task
        Try
            Dim sujet = "🔔 Nouvelle Notification"
            Dim message = "Vous avez reçu une <strong>notification</strong> importante." &
                         vbCrLf & vbCrLf &
                         "💜 Détails du style :" &
                         vbCrLf & "• Couleur : Violet (#7b1fa2)" &
                         vbCrLf & "• Icône : 🔔" &
                         vbCrLf & "• Gradient : #7b1fa2 → #9c27b0" &
                         vbCrLf & vbCrLf &
                         "Parfait pour :" &
                         vbCrLf & "• Notifications d'application" &
                         vbCrLf & "• Rappels automatiques" &
                         vbCrLf & "• Alertes de workflow"

            Console.Write("   Envoi en cours... ")
            Dim resultat = Await sender.EnvoyerEmailAsync(TO_EMAIL, sujet, message, TypeEmail.Notification)

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

    Private Async Function TestTousLesTypes(sender As EmailSender) As Task
        Try
            Dim sujet = "📊 Récapitulatif - 7 Types d'Emails"
            Dim message = "EmailSenderDLL v1.2.0 propose maintenant <strong>7 types d'emails</strong> :" &
                         vbCrLf & vbCrLf &
                         "🔵 <strong>Info</strong> - Bleu (#2196F3)" &
                         vbCrLf & "Pour les informations générales" &
                         vbCrLf & vbCrLf &
                         "🔴 <strong>Erreur</strong> - Rouge (#f44336)" &
                         vbCrLf & "Pour les erreurs et échecs" &
                         vbCrLf & vbCrLf &
                         "🟠 <strong>Urgence</strong> - Orange (#ff9800)" &
                         vbCrLf & "Pour les situations urgentes" &
                         vbCrLf & vbCrLf &
                         "🟢 <strong>Succès</strong> - Vert (#4caf50)" &
                         vbCrLf & "Pour confirmer les réussites" &
                         vbCrLf & vbCrLf &
                         "🔴 <strong>Alerte</strong> - Rouge foncé (#b71c1c)" &
                         vbCrLf & "Pour les incidents critiques" &
                         vbCrLf & vbCrLf &
                         "🟡 <strong>Avertissement</strong> - Jaune (#f57f17)" &
                         vbCrLf & "Pour les avertissements importants" &
                         vbCrLf & vbCrLf &
                         "🟣 <strong>Notification</strong> - Violet (#7b1fa2)" &
                         vbCrLf & "Pour les notifications automatiques"

            Dim signature = "🤖 EmailSenderDLL v1.2.0" & vbCrLf &
                           "Tech Dev DAAM" & vbCrLf &
                           "7 types | CC/BCC | Signatures | Pièces jointes"

            Console.Write("   Envoi en cours... ")
            Dim resultat = Await sender.EnvoyerEmailAsync(
                TO_EMAIL, sujet, message, TypeEmail.Info, signature
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
