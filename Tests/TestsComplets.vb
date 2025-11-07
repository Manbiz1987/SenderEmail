Imports System
Imports System.Collections.Generic
Imports EmailSenderDLL

''' <summary>
''' Classe de tests complète pour EmailSenderDLL v1.2.0
''' Teste tous les 7 types d'emails + fonctionnalités avancées
''' </summary>
Module TestsComplets
    ' ═══════════════════════════════════════════════════════════
    ' CONFIGURATION
    ' ═══════════════════════════════════════════════════════════
    Private Const API_KEY As String = "***REMOVED***"
    Private Const FROM_EMAIL As String = "***REMOVED***"
    Private Const FROM_NAME As String = "Tech Dev DAAM"
    Private Const TO_EMAIL As String = "***REMOVED***"

    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        AfficherEntete()

        Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)
        Dim testsPasses As Integer = 0
        Dim testsEchoues As Integer = 0

        ' Tests des 7 types d'emails
        If ExecuterTest("Info", Function() TestEmailInfo(sender)) Then testsPasses += 1 Else testsEchoues += 1
        If ExecuterTest("Erreur", Function() TestEmailErreur(sender)) Then testsPasses += 1 Else testsEchoues += 1
        If ExecuterTest("Urgence", Function() TestEmailUrgence(sender)) Then testsPasses += 1 Else testsEchoues += 1
        If ExecuterTest("Succès", Function() TestEmailSucces(sender)) Then testsPasses += 1 Else testsEchoues += 1
        If ExecuterTest("Alerte", Function() TestEmailAlerte(sender)) Then testsPasses += 1 Else testsEchoues += 1
        If ExecuterTest("Avertissement", Function() TestEmailAvertissement(sender)) Then testsPasses += 1 Else testsEchoues += 1
        If ExecuterTest("Notification", Function() TestEmailNotification(sender)) Then testsPasses += 1 Else testsEchoues += 1

        ' Tests fonctionnalités avancées
        If ExecuterTest("Email avec CC/BCC", Function() TestEmailAvecCopies(sender)) Then testsPasses += 1 Else testsEchoues += 1
        If ExecuterTest("Email avec signature", Function() TestEmailAvecSignature(sender)) Then testsPasses += 1 Else testsEchoues += 1
        If ExecuterTest("Email avec pièces jointes (affichage)", Function() TestEmailAvecPiecesJointesAffichage(sender)) Then testsPasses += 1 Else testsEchoues += 1

        AfficherResume(testsPasses, testsEchoues)
    End Sub

    ' ═══════════════════════════════════════════════════════════
    ' FONCTIONS D'AFFICHAGE
    ' ═══════════════════════════════════════════════════════════
    Private Sub AfficherEntete()
        Console.Clear()
        Console.WriteLine("╔═══════════════════════════════════════════════════════════╗")
        Console.WriteLine("║         TESTS COMPLETS - EmailSenderDLL v1.2.0           ║")
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝")
        Console.WriteLine()
    End Sub

    Private Function ExecuterTest(nom As String, test As Func(Of Task(Of Boolean))) As Boolean
        Console.Write($"📧 Test : {nom,-30} ")
        Try
            Dim resultat = test().Result
            If resultat Then
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine("✅ SUCCÈS")
                Console.ResetColor()
                System.Threading.Thread.Sleep(1500)
                Return True
            Else
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("❌ ÉCHEC")
                Console.ResetColor()
                Return False
            End If
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine($"❌ ERREUR: {ex.Message}")
            Console.ResetColor()
            Return False
        End Try
    End Function

    Private Sub AfficherResume(passes As Integer, echoues As Integer)
        Console.WriteLine()
        Console.WriteLine("╔═══════════════════════════════════════════════════════════╗")
        Console.WriteLine("║                    RÉSUMÉ DES TESTS                       ║")
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝")
        Console.WriteLine()
        Console.ForegroundColor = ConsoleColor.Green
        Console.WriteLine($"  ✅ Tests réussis : {passes}")
        Console.ResetColor()
        If echoues > 0 Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine($"  ❌ Tests échoués : {echoues}")
            Console.ResetColor()
        End If
        Console.WriteLine($"  📊 Total : {passes + echoues}")
        Console.WriteLine()
        Console.WriteLine($"  📧 Vérifiez votre boîte : {TO_EMAIL}")
        Console.WriteLine()
        Console.WriteLine("═══════════════════════════════════════════════════════════")
        Console.WriteLine()
        Console.WriteLine("Appuyez sur une touche pour quitter...")
        Console.ReadKey()
    End Sub

    ' ═══════════════════════════════════════════════════════════
    ' TESTS DES 7 TYPES D'EMAILS
    ' ═══════════════════════════════════════════════════════════
    Private Async Function TestEmailInfo(sender As EmailSender) As Task(Of Boolean)
        Return Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "ℹ️ Test Email Info",
            "Ceci est un email de type <strong>Info</strong> (Bleu #2196F3)." & vbCrLf &
            "Utilisé pour les informations générales.",
            TypeEmail.Info
        )
    End Function

    Private Async Function TestEmailErreur(sender As EmailSender) As Task(Of Boolean)
        Return Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "❌ Test Email Erreur",
            "Ceci est un email de type <strong>Erreur</strong> (Rouge #f44336)." & vbCrLf &
            "Utilisé pour signaler des erreurs.",
            TypeEmail.Erreur
        )
    End Function

    Private Async Function TestEmailUrgence(sender As EmailSender) As Task(Of Boolean)
        Return Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "⚡ Test Email Urgence",
            "Ceci est un email de type <strong>Urgence</strong> (Orange #ff9800)." & vbCrLf &
            "Utilisé pour les situations urgentes.",
            TypeEmail.Urgence
        )
    End Function

    Private Async Function TestEmailSucces(sender As EmailSender) As Task(Of Boolean)
        Return Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "✅ Test Email Succès",
            "Ceci est un email de type <strong>Succès</strong> (Vert #4caf50)." & vbCrLf &
            "Utilisé pour confirmer les réussites.",
            TypeEmail.Succes
        )
    End Function

    Private Async Function TestEmailAlerte(sender As EmailSender) As Task(Of Boolean)
        Return Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "🚨 Test Email Alerte",
            "Ceci est un email de type <strong>Alerte</strong> (Rouge foncé #b71c1c)." & vbCrLf &
            "Utilisé pour les incidents critiques.",
            TypeEmail.Alerte
        )
    End Function

    Private Async Function TestEmailAvertissement(sender As EmailSender) As Task(Of Boolean)
        Return Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "⚠️ Test Email Avertissement",
            "Ceci est un email de type <strong>Avertissement</strong> (Jaune #f57f17)." & vbCrLf &
            "Utilisé pour les actions requises.",
            TypeEmail.Avertissement
        )
    End Function

    Private Async Function TestEmailNotification(sender As EmailSender) As Task(Of Boolean)
        Return Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "🔔 Test Email Notification",
            "Ceci est un email de type <strong>Notification</strong> (Violet #7b1fa2)." & vbCrLf &
            "Utilisé pour les notifications automatiques.",
            TypeEmail.Notification
        )
    End Function

    ' ═══════════════════════════════════════════════════════════
    ' TESTS FONCTIONNALITÉS AVANCÉES
    ' ═══════════════════════════════════════════════════════════
    Private Async Function TestEmailAvecCopies(sender As EmailSender) As Task(Of Boolean)
        Dim cc As New List(Of String) From {"cc@example.com"}
        Dim cci As New List(Of String) From {"bcc@example.com"}

        Return Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "📎 Test Email avec CC/BCC",
            "Cet email est envoyé avec des copies CC et BCC." & vbCrLf &
            "• CC : cc@example.com" & vbCrLf &
            "• BCC : bcc@example.com",
            TypeEmail.Info,
            Nothing,
            Nothing,
            Nothing,
            cc,
            cci
        )
    End Function

    Private Async Function TestEmailAvecSignature(sender As EmailSender) As Task(Of Boolean)
        Dim signature = "Tech Dev DAAM" & vbCrLf &
                       "Développeur VB.NET" & vbCrLf &
                       "📧 ***REMOVED***" & vbCrLf &
                       "🌐 www.techdevdaam.com"

        Return Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "✍️ Test Email avec Signature",
            "Cet email contient une signature personnalisée en bas du message.",
            TypeEmail.Info,
            signature
        )
    End Function

    Private Async Function TestEmailAvecPiecesJointesAffichage(sender As EmailSender) As Task(Of Boolean)
        Dim pieces As New List(Of String) From {
            "Rapport_Mensuel.pdf",
            "Données_2025.xlsx",
            "Image_Graphique.png"
        }

        Return Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "📎 Test Email avec Pièces Jointes",
            "Cet email affiche une liste de pièces jointes (mode affichage).",
            TypeEmail.Info,
            Nothing,
            pieces
        )
    End Function

End Module
