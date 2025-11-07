Imports System
Imports EmailSenderDLL

Module TestOutlook
    Private Const API_KEY As String = "***REMOVED***"
    Private Const FROM_EMAIL As String = "***REMOVED***"
    Private Const FROM_NAME As String = "Tech Dev DAAM"
    Private Const TO_EMAIL As String = "***REMOVED***"

    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        Console.WriteLine("════════════════════════════════════════")
        Console.WriteLine("  TEST CORRECTION OUTLOOK - v1.2.1")
        Console.WriteLine("════════════════════════════════════════")
        Console.WriteLine()

        Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

        Console.WriteLine("📧 Test email Alerte")
        TestEmail(sender).Wait()

        Console.WriteLine()
        Console.WriteLine("════════════════════════════════════════")
        Console.WriteLine("  ✅ Email envoyé ! Vérifiez Outlook")
        Console.WriteLine("════════════════════════════════════════")
        Console.WriteLine()
        Console.ReadKey()
    End Sub

    Private Async Function TestEmail(sender As EmailSender) As Task
        Try
            Dim sujet = "🚨 TEST CORRECTION OUTLOOK"
            Dim message = "✅ Le titre devrait maintenant être visible en BLANC" &
                         vbCrLf & "✅ Le sujet personnalisé apparaît dans l'en-tête"

            Console.Write("   Envoi... ")
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
End Module
