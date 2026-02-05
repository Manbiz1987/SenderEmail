' ================================================================================
' FICHIER DE TEST - EmailSenderDLL
' ================================================================================
' 
' Ce fichier permet de tester rapidement l'envoi d'emails avec la DLL EmailSenderDLL
' 
' INSTRUCTIONS :
' 1. Remplacez les valeurs de configuration par vos vraies valeurs Resend
' 2. Remplacez les adresses emails de test par vos vraies adresses
' 3. Décommentez le test que vous voulez exécuter
' 4. Compilez et exécutez
' 
' ================================================================================

Imports EmailSenderDLL
Imports System.Threading.Tasks

Module TestEmail

    ' ============================================================================
    ' CONFIGURATION - Chargée depuis .env
    ' ============================================================================
    Private ReadOnly API_KEY As String
    Private ReadOnly FROM_EMAIL As String
    Private ReadOnly FROM_NAME As String
    Private ReadOnly TO_EMAIL As String
    
    ' Constructeur statique pour charger la configuration
    Sub New()
        Try
            EnvConfig.LoadEnvFile()
            API_KEY = EnvConfig.GetRequired("RESEND_API_KEY")
            FROM_EMAIL = EnvConfig.GetRequired("RESEND_FROM_EMAIL")
            FROM_NAME = EnvConfig.GetRequired("RESEND_FROM_NAME")
            TO_EMAIL = EnvConfig.GetOptional("TEST_TO_EMAIL", "test@example.com")
        Catch ex As Exception
            Console.WriteLine("❌ ERREUR: " & ex.Message)
            Console.WriteLine("Créez un fichier .env à partir de .env.example")
            Environment.Exit(1)
        End Try
    End Sub

    ' ============================================================================
    ' FONCTION PRINCIPALE
    ' ============================================================================
    Sub Main()
        Console.WriteLine("================================================================================")
        Console.WriteLine("                      TEST EmailSenderDLL")
        Console.WriteLine("================================================================================")
        Console.WriteLine()

        ' Exécuter les tests
        ExecuterTests().Wait()

        Console.WriteLine()
        Console.WriteLine("================================================================================")
        Console.WriteLine("                      Tests terminés !")
        Console.WriteLine("================================================================================")
        Console.WriteLine()
        Console.WriteLine("Appuyez sur Entrée pour quitter...")
        Console.ReadLine()
    End Sub

    ' ============================================================================
    ' EXÉCUTION DES TESTS
    ' ============================================================================
    Private Async Function ExecuterTests() As Task
        ' Test des 3 types d'emails
        Await Test2_EmailInfo()
        Await Task.Delay(500) ' Pause entre les envois
        Await Test3_EmailErreur()
        Await Task.Delay(500)
        Await Test4_EmailUrgence()
    End Function

    ' ============================================================================
    ' TEST 1 : Email Simple
    ' ============================================================================
    Private Async Function Test1_EmailSimple() As Task
        Console.WriteLine(">>> Test 1 : Email Simple")
        Console.WriteLine()

        Try
            Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

            Dim success = Await sender.EnvoyerEmailAsync(
                destinataire:=TO_EMAIL,
                sujet:="✅ Test EmailSenderDLL - Email Simple",
                message:="<p>Bonjour,</p>" &
                        "<p>Ceci est un test d'email simple envoyé avec <strong>EmailSenderDLL</strong>.</p>" &
                        "<p>Si vous recevez cet email, tout fonctionne correctement ! 🎉</p>"
            )

            AfficherResultat(success)

        Catch ex As Exception
            Console.WriteLine($"❌ Exception : {ex.Message}")
        End Try
    End Function

    ' ============================================================================
    ' TEST 2 : Email Info (Bleu)
    ' ============================================================================
    Private Async Function Test2_EmailInfo() As Task
        Console.WriteLine(">>> Test 2 : Email Info (Bleu)")
        Console.WriteLine()

        Try
            Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

            Dim success = Await sender.EnvoyerEmailAsync(
                destinataire:=TO_EMAIL,
                sujet:="📊 Test EmailSenderDLL - Email Info",
                message:="<p>Test du template <strong>Info</strong> (Bleu).</p>" &
                        "<p>Ce type d'email est utilisé pour les notifications informatives.</p>",
                typeEmail:=TypeEmail.Info
            )

            AfficherResultat(success)

        Catch ex As Exception
            Console.WriteLine($"❌ Exception : {ex.Message}")
        End Try
    End Function

    ' ============================================================================
    ' TEST 3 : Email Erreur (Rouge)
    ' ============================================================================
    Private Async Function Test3_EmailErreur() As Task
        Console.WriteLine(">>> Test 3 : Email Erreur (Rouge)")
        Console.WriteLine()

        Try
            Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

            Dim success = Await sender.EnvoyerEmailAsync(
                destinataire:=TO_EMAIL,
                sujet:="⚠️ Test EmailSenderDLL - Email Erreur",
                message:="<p>Test du template <strong>Erreur</strong> (Rouge).</p>" &
                        "<p>Ce type d'email est utilisé pour les alertes d'erreurs.</p>" &
                        "<p><strong>Exemple :</strong> Échec de connexion à la base de données.</p>",
                typeEmail:=TypeEmail.Erreur
            )

            AfficherResultat(success)

        Catch ex As Exception
            Console.WriteLine($"❌ Exception : {ex.Message}")
        End Try
    End Function

    ' ============================================================================
    ' TEST 4 : Email Urgence (Orange)
    ' ============================================================================
    Private Async Function Test4_EmailUrgence() As Task
        Console.WriteLine(">>> Test 4 : Email Urgence (Orange)")
        Console.WriteLine()

        Try
            Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

            Dim success = Await sender.EnvoyerEmailAsync(
                destinataire:=TO_EMAIL,
                sujet:="🚨 Test EmailSenderDLL - Email Urgence",
                message:="<p>Test du template <strong>Urgence</strong> (Orange).</p>" &
                        "<p>Ce type d'email est utilisé pour les messages urgents nécessitant une action immédiate.</p>" &
                        "<p><strong>Exemple :</strong> Serveur principal hors ligne.</p>",
                typeEmail:=TypeEmail.Urgence
            )

            AfficherResultat(success)

        Catch ex As Exception
            Console.WriteLine($"❌ Exception : {ex.Message}")
        End Try
    End Function

    ' ============================================================================
    ' TEST 5 : Email avec Signature
    ' ============================================================================
    Private Async Function Test5_EmailAvecSignature() As Task
        Console.WriteLine(">>> Test 5 : Email avec Signature Personnalisée")
        Console.WriteLine()

        Try
            Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

            Dim signature As String = "Cordialement," & vbCrLf &
                                     "Mohamed Ben El Khouja" & vbCrLf &
                                     "Tech Dev DAAM" & vbCrLf &
                                     "Email : ***REMOVED***" & vbCrLf &
                                     "Tel : +216 XX XXX XXX"

            Dim success = Await sender.EnvoyerEmailAsync(
                destinataire:=TO_EMAIL,
                sujet:="✍️ Test EmailSenderDLL - Email avec Signature",
                message:="<p>Cet email contient une signature personnalisée.</p>" &
                        "<p>Regardez en bas de l'email pour voir la signature.</p>",
                typeEmail:=TypeEmail.Info,
                signature:=signature
            )

            AfficherResultat(success)

        Catch ex As Exception
            Console.WriteLine($"❌ Exception : {ex.Message}")
        End Try
    End Function

    ' ============================================================================
    ' TEST 6 : Email avec CC
    ' ============================================================================
    Private Async Function Test6_EmailAvecCC() As Task
        Console.WriteLine(">>> Test 6 : Email avec CC (Copie Carbone)")
        Console.WriteLine()

        Try
            Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

            Dim cc As New List(Of String) From {
                "***REMOVED***"
            }

            Dim success = Await sender.EnvoyerEmailAsync(
                destinataire:=TO_EMAIL,
                sujet:="📧 Test EmailSenderDLL - Email avec CC",
                message:="<p>Cet email est envoyé avec une copie carbone (CC).</p>" &
                        "<p>L'adresse en CC recevra également cet email et sera visible par tous les destinataires.</p>",
                typeEmail:=TypeEmail.Info,
                cc:=cc
            )

            AfficherResultat(success)

        Catch ex As Exception
            Console.WriteLine($"❌ Exception : {ex.Message}")
        End Try
    End Function

    ' ============================================================================
    ' TEST 7 : Email avec BCC
    ' ============================================================================
    Private Async Function Test7_EmailAvecBCC() As Task
        Console.WriteLine(">>> Test 7 : Email avec BCC (Copie Carbone Invisible)")
        Console.WriteLine()

        Try
            Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

            Dim bcc As New List(Of String) From {
                "benkhouja.mahmoud@hotmail.fr"
            }

            Dim success = Await sender.EnvoyerEmailAsync(
                destinataire:=TO_EMAIL,
                sujet:="🔒 Test EmailSenderDLL - Email avec BCC",
                message:="<p>Cet email est envoyé avec une copie carbone invisible (BCC).</p>" &
                        "<p>L'adresse en BCC recevra également cet email mais ne sera PAS visible par les autres destinataires.</p>",
                typeEmail:=TypeEmail.Info,
                cci:=bcc
            )

            AfficherResultat(success)

        Catch ex As Exception
            Console.WriteLine($"❌ Exception : {ex.Message}")
        End Try
    End Function

    ' ============================================================================
    ' TEST 8 : Email Complet (Toutes les options)
    ' ============================================================================
    Private Async Function Test8_EmailComplet() As Task
        Console.WriteLine(">>> Test 8 : Email Complet (Toutes les options)")
        Console.WriteLine()

        Try
            Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

            Dim cc As New List(Of String) From {"***REMOVED***"}
            Dim bcc As New List(Of String) From {"benkhouja.mahmoud@hotmail.fr"}
            Dim pieceJointes As New List(Of String) From {
                "Rapport_Test.pdf",
                "Resultats_Analyse.xlsx"
            }
            Dim signature As String = "Cordialement," & vbCrLf & "L'équipe Tech Dev DAAM"

            Dim success = Await sender.EnvoyerEmailAsync(
                destinataire:=TO_EMAIL,
                sujet:="🎯 Test EmailSenderDLL - Email Complet",
                message:="<p>Cet email utilise <strong>toutes les fonctionnalités</strong> de EmailSenderDLL :</p>" &
                        "<ul>" &
                        "<li>✅ Template Urgence (Orange)</li>" &
                        "<li>✅ Signature personnalisée</li>" &
                        "<li>✅ Pièces jointes (affichage)</li>" &
                        "<li>✅ CC (Copie carbone)</li>" &
                        "<li>✅ BCC (Copie cachée)</li>" &
                        "</ul>" &
                        "<p>Si vous voyez tous ces éléments, la DLL fonctionne parfaitement ! 🎉</p>",
                typeEmail:=TypeEmail.Urgence,
                signature:=signature,
                pieceJointes:=pieceJointes,
                cc:=cc,
                cci:=bcc
            )

            AfficherResultat(success)

        Catch ex As Exception
            Console.WriteLine($"❌ Exception : {ex.Message}")
        End Try
    End Function

    ' ============================================================================
    ' FONCTION UTILITAIRE : Afficher le résultat
    ' ============================================================================
    Private Sub AfficherResultat(success As Boolean)
        Console.WriteLine()
        If success Then
            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine("✅ Email envoyé avec SUCCÈS !")
            Console.WriteLine("   Vérifiez votre boîte mail (et le dossier spam si nécessaire)")
        Else
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("❌ ÉCHEC de l'envoi de l'email")
            Console.WriteLine("   Consultez les messages d'erreur ci-dessus")
        End If
        Console.ResetColor()
        Console.WriteLine()
    End Sub

End Module
