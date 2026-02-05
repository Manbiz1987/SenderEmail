Imports System
Imports EmailSenderDLL

Module TestTousTypes
    Private ReadOnly API_KEY As String
    Private ReadOnly FROM_EMAIL As String
    Private ReadOnly FROM_NAME As String
    Private ReadOnly TO_EMAIL As String
    
    ' Constructeur statique
    Sub New()
        Try
            EnvConfig.LoadEnvFile()
            API_KEY = EnvConfig.GetRequired("RESEND_API_KEY")
            FROM_EMAIL = EnvConfig.GetRequired("RESEND_FROM_EMAIL")
            FROM_NAME = EnvConfig.GetRequired("RESEND_FROM_NAME")
            TO_EMAIL = EnvConfig.GetOptional("TEST_TO_EMAIL", "test@example.com")
        Catch ex As Exception
            Console.WriteLine("❌ ERREUR: " & ex.Message)
            Environment.Exit(1)
        End Try
    End Sub

    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        Console.WriteLine("═══════════════════════════════════════════════")
        Console.WriteLine("  TEST TOUS LES 7 TYPES - v1.2.1 OUTLOOK")
        Console.WriteLine("═══════════════════════════════════════════════")
        Console.WriteLine()

        Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

        Console.WriteLine("1️⃣  Test Info (Bleu)")
        TestType(sender, TypeEmail.Info, "ℹ️ Information Importante").Wait()
        System.Threading.Thread.Sleep(1500)

        Console.WriteLine("2️⃣  Test Erreur (Rouge)")
        TestType(sender, TypeEmail.Erreur, "❌ Erreur Système").Wait()
        System.Threading.Thread.Sleep(1500)

        Console.WriteLine("3️⃣  Test Urgence (Orange)")
        TestType(sender, TypeEmail.Urgence, "⚡ Action Urgente Requise").Wait()
        System.Threading.Thread.Sleep(1500)

        Console.WriteLine("4️⃣  Test Succès (Vert)")
        TestType(sender, TypeEmail.Succes, "✅ Opération Réussie").Wait()
        System.Threading.Thread.Sleep(1500)

        Console.WriteLine("5️⃣  Test Alerte (Rouge foncé)")
        TestType(sender, TypeEmail.Alerte, "🚨 ALERTE CRITIQUE").Wait()
        System.Threading.Thread.Sleep(1500)

        Console.WriteLine("6️⃣  Test Avertissement (Jaune)")
        TestType(sender, TypeEmail.Avertissement, "⚠️ ⚠️ Alerte de validation pour la déclaration ACM").Wait()
        System.Threading.Thread.Sleep(1500)

        Console.WriteLine("7️⃣  Test Notification (Violet)")
        TestType(sender, TypeEmail.Notification, "🔔 Nouvelle Notification").Wait()

        Console.WriteLine()
        Console.WriteLine("═══════════════════════════════════════════════")
        Console.WriteLine("  ✅ 7 EMAILS ENVOYÉS !")
        Console.WriteLine("  Vérifiez Outlook : " & TO_EMAIL)
        Console.WriteLine("═══════════════════════════════════════════════")
        Console.WriteLine()
        Console.ReadKey()
    End Sub

    Private Async Function TestType(sender As EmailSender, typeEmail As TypeEmail, sujet As String) As Task
        Try
            Dim message = "Test du header avec couleur unie pour compatibilité Outlook." &
                         vbCrLf & vbCrLf &
                         "✅ Fond coloré visible" &
                         vbCrLf & "✅ Texte blanc lisible" &
                         vbCrLf & "✅ Sujet affiché correctement"

            Console.Write("   Envoi... ")
            Dim resultat = Await sender.EnvoyerEmailAsync(TO_EMAIL, sujet, message, typeEmail)

            If resultat Then
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine("✅")
                Console.ResetColor()
            Else
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("❌")
                Console.ResetColor()
            End If
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("❌ " & ex.Message)
            Console.ResetColor()
        End Try
    End Function
End Module
