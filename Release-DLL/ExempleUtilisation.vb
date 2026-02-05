Imports EmailSenderDLL

''' <summary>
''' Exemple d'utilisation de EmailSenderDLL v2.0.0
''' </summary>
Module ExempleUtilisation
    Sub Main()
        Console.WriteLine("===========================================")
        Console.WriteLine("EmailSenderDLL v2.0.0 - Exemples")
        Console.WriteLine("===========================================")
        Console.WriteLine()
        
        ' ========================================
        ' EXEMPLE 1 : SMTP Ultra-Simplifié
        ' ========================================
        Console.WriteLine("Exemple 1 : Envoi SMTP ultra-simplifié")
        Console.WriteLine("---------------------------------------")
        
        Try
            ' Création du sender (charge automatiquement depuis .env)
            Dim sender As New EmailSenderSMTP()
            
            ' Envoi simple (tout depuis .env)
            sender.EnvoyerEmailAsync(
                message:="<h2>Test EmailSenderDLL</h2><p>Ceci est un email de test.</p>"
            ).Wait()
            
            Console.WriteLine("✓ Email envoyé avec succès !")
        Catch ex As Exception
            Console.WriteLine($"✗ Erreur : {ex.Message}")
        End Try
        
        Console.WriteLine()
        Console.WriteLine()
        
        ' ========================================
        ' EXEMPLE 2 : Tous les types d'emails
        ' ========================================
        Console.WriteLine("Exemple 2 : Différents types d'emails")
        Console.WriteLine("-------------------------------------")
        
        Try
            Dim sender As New EmailSenderSMTP()
            
            ' Email Info
            sender.EnvoyerEmailAsync(
                message:="Information importante",
                sujet:="Email Info",
                typeEmail:=TypeEmail.Info
            ).Wait()
            Console.WriteLine("✓ Email Info envoyé")
            
            ' Email Succès
            sender.EnvoyerEmailAsync(
                message:="Opération réussie !",
                sujet:="Email Succès",
                typeEmail:=TypeEmail.Succes
            ).Wait()
            Console.WriteLine("✓ Email Succès envoyé")
            
            ' Email Urgence
            sender.EnvoyerEmailAsync(
                message:="Action immédiate requise",
                sujet:="Email Urgent",
                typeEmail:=TypeEmail.Urgence
            ).Wait()
            Console.WriteLine("✓ Email Urgence envoyé (Priorité Haute)")
            
        Catch ex As Exception
            Console.WriteLine($"✗ Erreur : {ex.Message}")
        End Try
        
        Console.WriteLine()
        Console.WriteLine()
        
        ' ========================================
        ' EXEMPLE 3 : Avec pièces jointes
        ' ========================================
        Console.WriteLine("Exemple 3 : Email avec pièces jointes")
        Console.WriteLine("--------------------------------------")
        
        Try
            Dim sender As New EmailSenderSMTP()
            
            sender.EnvoyerEmailAsync(
                message:="<h2>Rapport mensuel</h2><p>Veuillez trouver ci-joint le rapport.</p>",
                destinataire:="destinataire@email.com",
                sujet:="Rapport Janvier 2026",
                typeEmail:=TypeEmail.Info,
                signature:="Cordialement,<br>MBTI Consult",
                fichiersAttaches:=New List(Of String) From {
                    "C:\Rapports\rapport.pdf",
                    "C:\Rapports\donnees.xlsx"
                }
            ).Wait()
            
            Console.WriteLine("✓ Email avec pièces jointes envoyé")
            
        Catch ex As Exception
            Console.WriteLine($"✗ Erreur : {ex.Message}")
        End Try
        
        Console.WriteLine()
        Console.WriteLine()
        
        ' ========================================
        ' EXEMPLE 4 : Avec CC et BCC
        ' ========================================
        Console.WriteLine("Exemple 4 : Email avec CC et BCC")
        Console.WriteLine("---------------------------------")
        
        Try
            Dim sender As New EmailSenderSMTP()
            
            sender.EnvoyerEmailAsync(
                message:="Notification importante",
                destinataire:="principal@email.com",
                sujet:="Notification",
                typeEmail:=TypeEmail.Notification,
                cc:=New List(Of String) From {"cc1@email.com", "cc2@email.com"},
                cci:=New List(Of String) From {"archive@email.com"}
            ).Wait()
            
            Console.WriteLine("✓ Email avec CC et BCC envoyé")
            
        Catch ex As Exception
            Console.WriteLine($"✗ Erreur : {ex.Message}")
        End Try
        
        Console.WriteLine()
        Console.WriteLine()
        Console.WriteLine("===========================================")
        Console.WriteLine("Exemples terminés")
        Console.WriteLine("===========================================")
        Console.WriteLine()
        Console.WriteLine("Appuyez sur une touche pour quitter...")
        Console.ReadKey()
    End Sub
End Module
