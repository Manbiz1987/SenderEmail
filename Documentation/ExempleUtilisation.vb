' ================================================================================
' EXEMPLES D'UTILISATION - EmailSenderDLL
' ================================================================================
' 
' Ce fichier contient des exemples complets d'utilisation de la DLL EmailSenderDLL
' pour envoyer des emails via SendGrid avec différents types de templates.
'
' ================================================================================

Imports EmailSenderDLL
Imports System.Collections.Generic
Imports System.Threading.Tasks

Public Class ExemplesEmailSender

    ' Configuration SendGrid (À REMPLACER par vos vraies valeurs)
    Private Const API_KEY As String = "SG.VOTRE_CLE_API_SENDGRID"
    Private Const FROM_EMAIL As String = "votre.email@example.com"
    Private Const FROM_NAME As String = "Votre Nom"

    ' ============================================================================
    ' EXEMPLE 1 : Email Simple (Minimal)
    ' ============================================================================
    Public Async Function Exemple1_EmailSimple() As Task
        Console.WriteLine("=== EXEMPLE 1 : Email Simple ===")

        Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

        Dim success = Await sender.EnvoyerEmailAsync(
            destinataire:="destinataire@example.com",
            sujet:="Test Email Simple",
            message:="<p>Bonjour,</p><p>Ceci est un email de test simple.</p>"
        )

        If success Then
            Console.WriteLine("✅ Email envoyé avec succès !")
        Else
            Console.WriteLine("❌ Erreur lors de l'envoi")
        End If
    End Function

    ' ============================================================================
    ' EXEMPLE 2 : Email Info (Bleu)
    ' ============================================================================
    Public Async Function Exemple2_EmailInfo() As Task
        Console.WriteLine(vbCrLf & "=== EXEMPLE 2 : Email Info (Bleu) ===")

        Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

        Dim success = Await sender.EnvoyerEmailAsync(
            destinataire:="destinataire@example.com",
            sujet:="📊 Rapport de Backup",
            message:="<p><strong>Le backup automatique s'est terminé avec succès.</strong></p>" &
                    "<p>Détails :</p>" &
                    "<ul>" &
                    "<li>Heure : 02:00 AM</li>" &
                    "<li>Taille : 2.5 GB</li>" &
                    "<li>Durée : 15 minutes</li>" &
                    "</ul>",
            typeEmail:=TypeEmail.Info
        )

        If success Then Console.WriteLine("✅ Email Info envoyé")
    End Function

    ' ============================================================================
    ' EXEMPLE 3 : Email Erreur (Rouge)
    ' ============================================================================
    Public Async Function Exemple3_EmailErreur() As Task
        Console.WriteLine(vbCrLf & "=== EXEMPLE 3 : Email Erreur (Rouge) ===")

        Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

        Dim success = Await sender.EnvoyerEmailAsync(
            destinataire:="admin@example.com",
            sujet:="⚠️ ERREUR : Connexion Base de Données",
            message:="<p><strong>Une erreur critique s'est produite !</strong></p>" &
                    "<p>Le système n'a pas pu se connecter à la base de données principale.</p>" &
                    "<p><strong>Code d'erreur :</strong> DB_CONNECTION_TIMEOUT</p>" &
                    "<p><strong>Action requise :</strong> Vérifier la disponibilité du serveur SQL.</p>",
            typeEmail:=TypeEmail.Erreur
        )

        If success Then Console.WriteLine("✅ Email Erreur envoyé")
    End Function

    ' ============================================================================
    ' EXEMPLE 4 : Email Urgence (Orange)
    ' ============================================================================
    Public Async Function Exemple4_EmailUrgence() As Task
        Console.WriteLine(vbCrLf & "=== EXEMPLE 4 : Email Urgence (Orange) ===")

        Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

        Dim success = Await sender.EnvoyerEmailAsync(
            destinataire:="direction@example.com",
            sujet:="🚨 URGENT : Intervention Requise",
            message:="<p><strong>Votre intervention immédiate est requise !</strong></p>" &
                    "<p>Le serveur de production principal est en surcharge.</p>" &
                    "<p><strong>Statut actuel :</strong></p>" &
                    "<ul>" &
                    "<li>CPU : 95%</li>" &
                    "<li>RAM : 89%</li>" &
                    "<li>Disque : 92%</li>" &
                    "</ul>" &
                    "<p>Merci de prendre les mesures nécessaires dans les plus brefs délais.</p>",
            typeEmail:=TypeEmail.Urgence
        )

        If success Then Console.WriteLine("✅ Email Urgence envoyé")
    End Function

    ' ============================================================================
    ' EXEMPLE 5 : Email avec Signature Personnalisée
    ' ============================================================================
    Public Async Function Exemple5_EmailAvecSignature() As Task
        Console.WriteLine(vbCrLf & "=== EXEMPLE 5 : Email avec Signature ===")

        Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

        Dim signature As String = "Cordialement," & vbCrLf &
                                 "L'équipe Tech Dev DAAM" & vbCrLf &
                                 "Email : support@daam.com.tn" & vbCrLf &
                                 "Tel : +216 XX XXX XXX"

        Dim success = Await sender.EnvoyerEmailAsync(
            destinataire:="client@example.com",
            sujet:="Bienvenue chez DAAM",
            message:="<p>Bonjour et bienvenue !</p>" &
                    "<p>Nous sommes ravis de vous compter parmi nos clients.</p>" &
                    "<p>N'hésitez pas à nous contacter si vous avez des questions.</p>",
            typeEmail:=TypeEmail.Info,
            signature:=signature
        )

        If success Then Console.WriteLine("✅ Email avec signature envoyé")
    End Function

    ' ============================================================================
    ' EXEMPLE 6 : Email avec Pièces Jointes (affichage uniquement)
    ' ============================================================================
    Public Async Function Exemple6_EmailAvecPiecesJointes() As Task
        Console.WriteLine(vbCrLf & "=== EXEMPLE 6 : Email avec Pièces Jointes ===")

        Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

        Dim pieceJointes As New List(Of String) From {
            "Rapport_Financier_Q4_2025.pdf",
            "Graphiques_Ventes.xlsx",
            "Presentation_Resultats.pptx"
        }

        Dim success = Await sender.EnvoyerEmailAsync(
            destinataire:="direction@example.com",
            sujet:="Rapport Trimestriel Q4 2025",
            message:="<p>Bonjour,</p>" &
                    "<p>Veuillez trouver ci-joint le rapport trimestriel avec les résultats du Q4 2025.</p>" &
                    "<p>Les chiffres sont très encourageants, avec une croissance de 15% par rapport au trimestre précédent.</p>",
            typeEmail:=TypeEmail.Info,
            pieceJointes:=pieceJointes
        )

        If success Then Console.WriteLine("✅ Email avec pièces jointes envoyé")
    End Function

    ' ============================================================================
    ' EXEMPLE 7 : Email avec CC (Copie Carbone)
    ' ============================================================================
    Public Async Function Exemple7_EmailAvecCC() As Task
        Console.WriteLine(vbCrLf & "=== EXEMPLE 7 : Email avec CC ===")

        Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

        Dim listeCC As New List(Of String) From {
            "manager@example.com",
            "responsable@example.com"
        }

        Dim success = Await sender.EnvoyerEmailAsync(
            destinataire:="employe@example.com",
            sujet:="Réunion Mensuelle - 15 Novembre",
            message:="<p>Bonjour,</p>" &
                    "<p>La réunion mensuelle aura lieu le <strong>15 novembre à 14h00</strong> dans la salle de conférence A.</p>" &
                    "<p><strong>Ordre du jour :</strong></p>" &
                    "<ul>" &
                    "<li>Bilan du mois écoulé</li>" &
                    "<li>Objectifs pour le mois prochain</li>" &
                    "<li>Questions diverses</li>" &
                    "</ul>",
            typeEmail:=TypeEmail.Info,
            cc:=listeCC
        )

        If success Then Console.WriteLine("✅ Email avec CC envoyé")
    End Function

    ' ============================================================================
    ' EXEMPLE 8 : Email avec BCC (Copie Carbone Invisible)
    ' ============================================================================
    Public Async Function Exemple8_EmailAvecBCC() As Task
        Console.WriteLine(vbCrLf & "=== EXEMPLE 8 : Email avec BCC ===")

        Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

        Dim listeBCC As New List(Of String) From {
            "archive@example.com",
            "backup@example.com"
        }

        Dim success = Await sender.EnvoyerEmailAsync(
            destinataire:="client@example.com",
            sujet:="Confirmation de Commande #12345",
            message:="<p>Bonjour,</p>" &
                    "<p>Votre commande #12345 a bien été enregistrée.</p>" &
                    "<p><strong>Détails :</strong></p>" &
                    "<ul>" &
                    "<li>Date : 31 octobre 2025</li>" &
                    "<li>Montant : 250.00 €</li>" &
                    "<li>Livraison estimée : 5-7 jours</li>" &
                    "</ul>",
            typeEmail:=TypeEmail.Info,
            cci:=listeBCC
        )

        If success Then Console.WriteLine("✅ Email avec BCC envoyé")
    End Function

    ' ============================================================================
    ' EXEMPLE 9 : Email Complet (Toutes les options)
    ' ============================================================================
    Public Async Function Exemple9_EmailComplet() As Task
        Console.WriteLine(vbCrLf & "=== EXEMPLE 9 : Email Complet (Toutes les options) ===")

        Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

        Dim listeCC As New List(Of String) From {
            "responsable.projet@example.com",
            "chef.equipe@example.com"
        }

        Dim listeBCC As New List(Of String) From {
            "direction@example.com"
        }

        Dim pieceJointes As New List(Of String) From {
            "Cahier_Charges.pdf",
            "Planning_Projet.xlsx",
            "Budget_Previsionnel.xlsx"
        }

        Dim signature As String = "Cordialement," & vbCrLf &
                                 "Mohamed Ben El Khouja" & vbCrLf &
                                 "Chef de Projet" & vbCrLf &
                                 "Tech Dev DAAM" & vbCrLf &
                                 "Tel : +216 XX XXX XXX"

        Dim success = Await sender.EnvoyerEmailAsync(
            destinataire:="client.principal@example.com",
            sujet:="🚀 Lancement Projet ABC - Documentation Complète",
            message:="<p>Bonjour,</p>" &
                    "<p>Nous sommes ravis de lancer officiellement le projet ABC.</p>" &
                    "<p><strong>Prochaines étapes :</strong></p>" &
                    "<ol>" &
                    "<li>Réunion de lancement : 5 novembre 2025</li>" &
                    "<li>Phase de conception : 6-20 novembre</li>" &
                    "<li>Développement : 21 novembre - 15 décembre</li>" &
                    "<li>Tests et livraison : 16-31 décembre</li>" &
                    "</ol>" &
                    "<p>Vous trouverez tous les documents nécessaires listés ci-dessous.</p>",
            typeEmail:=TypeEmail.Urgence,
            signature:=signature,
            pieceJointes:=pieceJointes,
            cc:=listeCC,
            cci:=listeBCC
        )

        If success Then Console.WriteLine("✅ Email complet envoyé")
    End Function

    ' ============================================================================
    ' EXEMPLE 10 : Envoi Multiple (Boucle)
    ' ============================================================================
    Public Async Function Exemple10_EnvoiMultiple() As Task
        Console.WriteLine(vbCrLf & "=== EXEMPLE 10 : Envoi Multiple ===")

        Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

        Dim destinataires As New List(Of String) From {
            "employe1@example.com",
            "employe2@example.com",
            "employe3@example.com"
        }

        Dim compteurSuccess As Integer = 0

        For Each destinataire In destinataires
            Dim success = Await sender.EnvoyerEmailAsync(
                destinataire:=destinataire,
                sujet:="Newsletter Novembre 2025",
                message:="<p>Bonjour,</p>" &
                        "<p>Voici votre newsletter mensuelle avec toutes les actualités de l'entreprise.</p>" &
                        "<p><strong>Au programme ce mois-ci :</strong></p>" &
                        "<ul>" &
                        "<li>Nouveaux projets lancés</li>" &
                        "<li>Anniversaires du mois</li>" &
                        "<li>Événements à venir</li>" &
                        "</ul>",
                typeEmail:=TypeEmail.Info
            )

            If success Then
                compteurSuccess += 1
                Console.WriteLine($"✅ Email envoyé à {destinataire}")
            Else
                Console.WriteLine($"❌ Échec pour {destinataire}")
            End If

            ' Pause de 100ms entre chaque envoi
            Await Task.Delay(100)
        Next

        Console.WriteLine($"{vbCrLf}📊 Résultat : {compteurSuccess}/{destinataires.Count} emails envoyés avec succès")
    End Function

    ' ============================================================================
    ' MAIN : Exécuter tous les exemples
    ' ============================================================================
    Public Shared Async Function Main() As Task
        Console.WriteLine("================================================================================")
        Console.WriteLine("               EXEMPLES D'UTILISATION - EmailSenderDLL")
        Console.WriteLine("================================================================================")
        Console.WriteLine()

        Dim exemples As New ExemplesEmailSender()

        ' Décommentez les exemples que vous voulez tester :

        'Await exemples.Exemple1_EmailSimple()
        'Await exemples.Exemple2_EmailInfo()
        'Await exemples.Exemple3_EmailErreur()
        'Await exemples.Exemple4_EmailUrgence()
        'Await exemples.Exemple5_EmailAvecSignature()
        'Await exemples.Exemple6_EmailAvecPiecesJointes()
        'Await exemples.Exemple7_EmailAvecCC()
        'Await exemples.Exemple8_EmailAvecBCC()
        'Await exemples.Exemple9_EmailComplet()
        'Await exemples.Exemple10_EnvoiMultiple()

        Console.WriteLine(vbCrLf & "================================================================================")
        Console.WriteLine("                    Exemples terminés !")
        Console.WriteLine("================================================================================")
        Console.ReadLine()
    End Function

End Class
