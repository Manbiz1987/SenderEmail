Imports System
Imports System.Collections.Generic
Imports EmailSenderDLL

''' <summary>
''' Exemple complet d'utilisation de EmailSenderDLL v1.2.0
''' Ce fichier démontre toutes les fonctionnalités disponibles
''' </summary>
Module ExempleUtilisation

    ' Configuration SendGrid
    Private Const API_KEY As String = "VOTRE_CLE_API_SENDGRID"
    Private Const FROM_EMAIL As String = "votre@email.com"
    Private Const FROM_NAME As String = "Votre Nom"
    Private Const TO_EMAIL As String = "destinataire@example.com"

    Sub Main()
        Console.WriteLine("═══════════════════════════════════════════════════")
        Console.WriteLine("  EXEMPLES - EmailSenderDLL v1.2.0")
        Console.WriteLine("═══════════════════════════════════════════════════")
        Console.WriteLine()

        ' ⚠️ IMPORTANT : Remplacez les constantes ci-dessus par vos vraies valeurs !

        ' Créer l'instance EmailSender
        Dim sender As New EmailSender(API_KEY, FROM_EMAIL, FROM_NAME)

        ' Décommentez les exemples que vous souhaitez tester

        ' Exemple1_EmailSimple(sender).Wait()
        ' Exemple2_EmailAvecSignature(sender).Wait()
        ' Exemple3_EmailAvecPieceJointe(sender).Wait()
        ' Exemple4_EmailAvecCCetBCC(sender).Wait()
        ' Exemple5_EmailSucces(sender).Wait()
        ' Exemple6_EmailAlerte(sender).Wait()
        ' Exemple7_EmailAvertissement(sender).Wait()
        ' Exemple8_EmailNotification(sender).Wait()
        ' Exemple9_EmailComplet(sender).Wait()

        Console.WriteLine()
        Console.WriteLine("Terminé ! Appuyez sur une touche...")
        Console.ReadKey()
    End Sub

    ''' <summary>
    ''' Exemple 1 : Email simple de type Info (Bleu)
    ''' </summary>
    Private Async Function Exemple1_EmailSimple(sender As EmailSender) As Task
        Console.WriteLine("📧 Exemple 1 : Email simple")
        Console.WriteLine("------------------------------------------")

        Dim resultat = Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "Test Email Simple",
            "Ceci est un email de test simple sans options avancées.",
            TypeEmail.Info
        )

        Console.WriteLine(If(resultat, "✅ Succès", "❌ Échec"))
        Console.WriteLine()
    End Function

    ''' <summary>
    ''' Exemple 2 : Email avec signature personnalisée
    ''' </summary>
    Private Async Function Exemple2_EmailAvecSignature(sender As EmailSender) As Task
        Console.WriteLine("📧 Exemple 2 : Email avec signature")
        Console.WriteLine("------------------------------------------")

        Dim signature = "Jean Dupont" & vbCrLf &
                       "Développeur VB.NET" & vbCrLf &
                       "Tech Dev DAAM" & vbCrLf &
                       "📧 jean.dupont@example.com" & vbCrLf &
                       "📱 +33 6 12 34 56 78"

        Dim resultat = Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "Email avec Signature",
            "Cet email contient une signature personnalisée en bas de page.",
            TypeEmail.Info,
            signature
        )

        Console.WriteLine(If(resultat, "✅ Succès", "❌ Échec"))
        Console.WriteLine()
    End Function

    ''' <summary>
    ''' Exemple 3 : Email avec pièce jointe réelle
    ''' </summary>
    Private Async Function Exemple3_EmailAvecPieceJointe(sender As EmailSender) As Task
        Console.WriteLine("📧 Exemple 3 : Email avec pièce jointe")
        Console.WriteLine("------------------------------------------")

        ' IMPORTANT : Remplacez par vos vrais chemins de fichiers
        Dim fichiers As New List(Of String) From {
            "C:\Documents\rapport.pdf",
            "C:\Documents\facture.xlsx"
        }

        Dim resultat = Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "Email avec Pièces Jointes",
            "Veuillez trouver ci-joint les documents demandés.",
            TypeEmail.Info,
            Nothing,        ' Pas de signature
            Nothing,        ' Pas de liste de noms
            fichiers        ' Fichiers réels à attacher
        )

        Console.WriteLine(If(resultat, "✅ Succès", "❌ Échec"))
        Console.WriteLine()
    End Function

    ''' <summary>
    ''' Exemple 4 : Email avec CC et BCC
    ''' </summary>
    Private Async Function Exemple4_EmailAvecCCetBCC(sender As EmailSender) As Task
        Console.WriteLine("📧 Exemple 4 : Email avec CC et BCC")
        Console.WriteLine("------------------------------------------")

        Dim cc As New List(Of String) From {
            "copie1@example.com",
            "copie2@example.com"
        }

        Dim bcc As New List(Of String) From {
            "copie.invisible@example.com"
        }

        Dim resultat = Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "Email avec CC et BCC",
            "Cet email est envoyé avec des copies visibles (CC) et invisibles (BCC).",
            TypeEmail.Info,
            Nothing,    ' signature
            Nothing,    ' pieceJointes
            Nothing,    ' fichiersAttaches
            cc,         ' CC
            bcc         ' BCC
        )

        Console.WriteLine(If(resultat, "✅ Succès", "❌ Échec"))
        Console.WriteLine()
    End Function

    ''' <summary>
    ''' Exemple 5 : Email de type Succès (Vert)
    ''' </summary>
    Private Async Function Exemple5_EmailSucces(sender As EmailSender) As Task
        Console.WriteLine("📧 Exemple 5 : Email Succès")
        Console.WriteLine("------------------------------------------")

        Dim message = "✅ <strong>Opération réussie !</strong>" & vbCrLf & vbCrLf &
                     "Votre commande #12345 a été traitée avec succès." & vbCrLf & vbCrLf &
                     "Détails :" & vbCrLf &
                     "• Date : " & DateTime.Now.ToString("dd/MM/yyyy HH:mm") & vbCrLf &
                     "• Statut : Confirmé" & vbCrLf &
                     "• Montant : 150,00 €"

        Dim resultat = Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "✅ Confirmation de commande",
            message,
            TypeEmail.Succes
        )

        Console.WriteLine(If(resultat, "✅ Succès", "❌ Échec"))
        Console.WriteLine()
    End Function

    ''' <summary>
    ''' Exemple 6 : Email de type Alerte (Rouge foncé)
    ''' </summary>
    Private Async Function Exemple6_EmailAlerte(sender As EmailSender) As Task
        Console.WriteLine("📧 Exemple 6 : Email Alerte")
        Console.WriteLine("------------------------------------------")

        Dim message = "🚨 <strong>ALERTE CRITIQUE</strong>" & vbCrLf & vbCrLf &
                     "Un incident de sécurité a été détecté sur le serveur." & vbCrLf & vbCrLf &
                     "⚠️ Actions requises :" & vbCrLf &
                     "• Vérifier les logs système" & vbCrLf &
                     "• Analyser les connexions suspectes" & vbCrLf &
                     "• Notifier l'équipe de sécurité" & vbCrLf & vbCrLf &
                     "Temps de détection : " & DateTime.Now.ToString("HH:mm:ss")

        Dim resultat = Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "🚨 ALERTE SÉCURITÉ",
            message,
            TypeEmail.Alerte
        )

        Console.WriteLine(If(resultat, "✅ Succès", "❌ Échec"))
        Console.WriteLine()
    End Function

    ''' <summary>
    ''' Exemple 7 : Email de type Avertissement (Jaune)
    ''' </summary>
    Private Async Function Exemple7_EmailAvertissement(sender As EmailSender) As Task
        Console.WriteLine("📧 Exemple 7 : Email Avertissement")
        Console.WriteLine("------------------------------------------")

        Dim message = "⚠️ <strong>Avertissement Important</strong>" & vbCrLf & vbCrLf &
                     "Votre quota de stockage atteint 85%." & vbCrLf & vbCrLf &
                     "📊 Détails :" & vbCrLf &
                     "• Espace utilisé : 850 GB" & vbCrLf &
                     "• Espace total : 1000 GB" & vbCrLf &
                     "• Espace restant : 150 GB" & vbCrLf & vbCrLf &
                     "💡 Recommandations :" & vbCrLf &
                     "• Supprimer les fichiers inutiles" & vbCrLf &
                     "• Archiver les anciennes données" & vbCrLf &
                     "• Envisager une augmentation de quota"

        Dim resultat = Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "⚠️ Quota de stockage",
            message,
            TypeEmail.Avertissement
        )

        Console.WriteLine(If(resultat, "✅ Succès", "❌ Échec"))
        Console.WriteLine()
    End Function

    ''' <summary>
    ''' Exemple 8 : Email de type Notification (Violet)
    ''' </summary>
    Private Async Function Exemple8_EmailNotification(sender As EmailSender) As Task
        Console.WriteLine("📧 Exemple 8 : Email Notification")
        Console.WriteLine("------------------------------------------")

        Dim message = "🔔 <strong>Nouvelle notification</strong>" & vbCrLf & vbCrLf &
                     "Vous avez un nouveau message dans votre espace client." & vbCrLf & vbCrLf &
                     "De : Service Client" & vbCrLf &
                     "Sujet : Mise à jour de votre dossier" & vbCrLf &
                     "Date : " & DateTime.Now.ToString("dd/MM/yyyy à HH:mm") & vbCrLf & vbCrLf &
                     "Connectez-vous à votre espace pour consulter le message."

        Dim resultat = Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "🔔 Nouveau message",
            message,
            TypeEmail.Notification
        )

        Console.WriteLine(If(resultat, "✅ Succès", "❌ Échec"))
        Console.WriteLine()
    End Function

    ''' <summary>
    ''' Exemple 9 : Email complet avec toutes les options
    ''' </summary>
    Private Async Function Exemple9_EmailComplet(sender As EmailSender) As Task
        Console.WriteLine("📧 Exemple 9 : Email complet")
        Console.WriteLine("------------------------------------------")

        Dim message = "Bonjour," & vbCrLf & vbCrLf &
                     "Voici votre <strong>rapport mensuel</strong> avec tous les documents joints." & vbCrLf & vbCrLf &
                     "📊 Résumé :" & vbCrLf &
                     "• Période : Novembre 2025" & vbCrLf &
                     "• Statut : Validé" & vbCrLf &
                     "• Documents : 2 fichiers joints"

        Dim signature = "Service Comptabilité" & vbCrLf &
                       "Tech Dev DAAM" & vbCrLf &
                       "📧 compta@example.com"

        Dim fichiers As New List(Of String) From {
            "C:\Rapports\rapport_novembre.pdf"
        }

        Dim cc As New List(Of String) From {"manager@example.com"}

        Dim resultat = Await sender.EnvoyerEmailAsync(
            TO_EMAIL,
            "📊 Rapport Mensuel - Novembre 2025",
            message,
            TypeEmail.Info,
            signature,
            Nothing,
            fichiers,
            cc
        )

        Console.WriteLine(If(resultat, "✅ Succès", "❌ Échec"))
        Console.WriteLine()
    End Function

End Module
