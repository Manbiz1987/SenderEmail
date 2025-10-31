# 📧 EmailSenderDLL - Envoi d'Emails via SendGrid

**DLL VB.NET pour envoyer des emails professionnels avec 3 types de templates HTML**

[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-blue)](https://dotnet.microsoft.com/)
[![Visual Basic](https://img.shields.io/badge/Visual%20Basic-.NET-blueviolet)](https://docs.microsoft.com/en-us/dotnet/visual-basic/)
[![SendGrid](https://img.shields.io/badge/SendGrid-API-green)](https://sendgrid.com/)

---

## 📋 Table des Matières

1. [Fonctionnalités](#-fonctionnalités)
2. [Installation](#-installation-rapide)
3. [Configuration SendGrid](#-configuration-sendgrid)
4. [Utilisation](#-utilisation-simple)
5. [Exemples Avancés](#-exemples-avancés)
6. [API Reference](#-api-reference)
7. [Gestion des Erreurs](#-gestion-des-erreurs)
8. [FAQ](#-faq)
9. [Support](#-support)

---

## ✨ Fonctionnalités

✅ **3 types d'emails** avec templates HTML distincts :
- 🔵 **Info** (Bleu #2196F3) - Pour les notifications informatives
- 🔴 **Erreur** (Rouge #f44336) - Pour les alertes d'erreurs
- 🟠 **Urgence** (Orange #ff9800) - Pour les messages urgents

✅ **Support complet CC et BCC** (copie carbone et copie cachée)  
✅ **Signatures personnalisables** pour chaque email  
✅ **Affichage de pièces jointes** (liste informative)  
✅ **Templates HTML responsive** adaptés aux mobiles  
✅ **API simple et intuitive** - seulement 3 lignes de code !  
✅ **Asynchrone** pour des performances optimales

---

## 🚀 Installation Rapide

### Option 1 : Utiliser la DLL compilée

1. **Téléchargez les fichiers** depuis `bin/Release/net48/` :
   - `EmailSenderDLL.dll`
   - `Newtonsoft.Json.dll`

2. **Ajoutez les références dans votre projet VB.NET** :
   - Clic droit sur votre projet → **Ajouter une référence**
   - Onglet **Parcourir** → Sélectionnez les 2 DLL
   - Cliquez **OK**

3. **C'est prêt !** Utilisez la DLL dans votre code

### Option 2 : Compiler depuis les sources

```bash
git clone <votre-repo>
cd SenderEmail
dotnet build -c Release
```

Les DLL compilées seront dans `bin/Release/net48/`

---

## 📖 Utilisation Simple

### Exemple minimal (3 lignes)

```vb
Imports EmailSenderDLL

' 1. Initialiser le sender
Dim sender As New EmailSender(
    "VOTRE_CLE_API_SENDGRID",
    "votre.email@example.com",
    "Votre Nom"
)

' 2. Envoyer un email
Dim success = Await sender.EnvoyerEmailAsync(
    destinataire:="destinataire@example.com",
    sujet:="Test Email",
    message:="<p>Bonjour, ceci est un test !</p>"
)

' 3. Vérifier le résultat
If success Then
    Console.WriteLine("✅ Email envoyé avec succès !")
Else
    Console.WriteLine("❌ Erreur lors de l'envoi")
End If
```

### Exemple avec toutes les options

```vb
' Créer des listes pour CC et BCC
Dim listeCC As New List(Of String) From {
    "copie1@example.com",
    "copie2@example.com"
}

Dim listeBCC As New List(Of String) From {
    "copie.cachee@example.com"
}

' Liste des pièces jointes (affichage uniquement)
Dim pieceJointes As New List(Of String) From {
    "Rapport_Q4_2025.pdf",
    "Graphiques.xlsx"
}

' Signature personnalisée
Dim signature As String = "Cordialement," & vbCrLf & 
                          "L'équipe Tech Dev DAAM" & vbCrLf &
                          "Tel: +216 XX XXX XXX"

' Envoyer un email d'erreur avec toutes les options
Dim success = Await sender.EnvoyerEmailAsync(
    destinataire:="destinataire@example.com",
    sujet:="⚠️ Erreur Critique Détectée",
    message:="<p>Une erreur a été détectée dans le système.</p><p><strong>Détails :</strong> Échec de la connexion à la base de données.</p>",
    typeEmail:=TypeEmail.Erreur,
    signature:=signature,
    pieceJointes:=pieceJointes,
    cc:=listeCC,
    cci:=listeBCC
)
```

## 📝 Exemples Avancés

### Email avec Signature
```vb
Dim signature As String = "Cordialement," & vbCrLf & 
                          "L'équipe Support" & vbCrLf &
                          "support@example.com"

Dim success = Await sender.EnvoyerEmailAsync(
    "client@example.com",
    "Confirmation d'inscription",
    "<p>Bienvenue ! Votre compte a été créé avec succès.</p>",
    TypeEmail.Info,
    signature:=signature
)
```

### Email Erreur
```vb
Dim success = Await sender.EnvoyerEmailAsync(
    "admin@example.com",
    "⚠️ Erreur Système",
    "<p><strong>Erreur détectée :</strong></p>" &
    "<p>La connexion à la base de données a échoué.</p>",
    TypeEmail.Erreur
)
```

### Email Urgence
```vb
Dim success = Await sender.EnvoyerEmailAsync(
    "direction@example.com",
    "🚨 URGENT : Action Requise",
    "<p>Intervention immédiate nécessaire.</p>",
    TypeEmail.Urgence
)
```

### Email avec Pièces Jointes
```vb
Dim pieces As New List(Of String) From {
    "Rapport_2025.pdf",
    "Graphiques.xlsx"
}

Dim success = Await sender.EnvoyerEmailAsync(
    "client@example.com",
    "Rapport Annuel",
    "<p>Veuillez trouver ci-joint le rapport.</p>",
    TypeEmail.Info,
    pieceJointes:=pieces
)
```

### Email avec CC et BCC
```vb
Dim cc As New List(Of String) From {"manager@example.com"}
Dim bcc As New List(Of String) From {"archive@example.com"}

Dim success = Await sender.EnvoyerEmailAsync(
    "employe@example.com",
    "Validation Congés",
    "<p>Votre demande a été approuvée.</p>",
    TypeEmail.Info,
    cc:=cc,
    cci:=bcc
)
```

### Email Complet (toutes les options)
```vb
Dim cc As New List(Of String) From {"copie@example.com"}
Dim bcc As New List(Of String) From {"archive@example.com"}
Dim pieces As New List(Of String) From {"Document.pdf"}
Dim signature As String = "Cordialement," & vbCrLf & "Support"

Dim success = Await sender.EnvoyerEmailAsync(
    destinataire:="client@example.com",
    sujet:="Dossier Complet",
    message:="<p>Voici votre dossier.</p>",
    typeEmail:=TypeEmail.Urgence,
    signature:=signature,
    pieceJointes:=pieces,
    cc:=cc,
    cci:=bcc
)
```

### Envoi en Boucle
```vb
Dim destinataires As New List(Of String) From {
    "user1@example.com",
    "user2@example.com",
    "user3@example.com"
}

For Each destinataire In destinataires
    Dim success = Await sender.EnvoyerEmailAsync(
        destinataire,
        "Newsletter",
        "<p>Votre newsletter mensuelle.</p>",
        TypeEmail.Info
    )
    
    If success Then
        Console.WriteLine($"✅ Envoyé à {destinataire}")
    End If
    
    Await Task.Delay(100) ' Pause de 100ms entre chaque envoi
Next
```

---

## ⚠️ Gestion des Erreurs

### Codes d'Erreur Courants

#### Erreur 401 : Unauthorized
**Cause :** Clé API invalide  
**Solution :** Vérifiez que votre clé commence par `SG.` et régénérez-la si nécessaire

#### Erreur 403 : Forbidden
**Cause :** Adresse expéditeur non validée  
**Solution :** Validez votre email dans SendGrid → Settings → Sender Authentication

#### Erreur 429 : Too Many Requests
**Cause :** Limite de quota dépassée  
**Solution :** Ajoutez des pauses entre les envois (`Await Task.Delay(100)`)

#### Email dans le Spam
**Cause :** Nouveau compte SendGrid  
**Solutions :**
- Demandez aux destinataires de marquer comme "Non spam"
- Validez votre domaine dans SendGrid (recommandé)
- Configurez SPF/DKIM/DMARC

### Exemple de Gestion d'Erreurs
```vb
Try
    Dim success = Await sender.EnvoyerEmailAsync(
        "destinataire@example.com",
        "Test",
        "<p>Message</p>"
    )
    
    If success Then
        Console.WriteLine("✅ Email envoyé")
    Else
        Console.WriteLine("❌ Échec de l'envoi")
    End If
    
Catch ex As Exception
    Console.WriteLine($"Exception : {ex.Message}")
End Try
```

---

## ❓ FAQ

### Q : Comment obtenir une clé API SendGrid ?
**R :** 
1. Créez un compte sur [sendgrid.com](https://sendgrid.com/)
2. Allez dans Settings → API Keys
3. Créez une clé avec Full Access
4. Copiez la clé générée (format: `SG.xxxxx...`)

### Q : Puis-je utiliser n'importe quelle adresse email ?
**R :** Non, vous devez valider l'adresse dans SendGrid (Settings → Sender Authentication).

### Q : Combien d'emails puis-je envoyer gratuitement ?
**R :** SendGrid offre 100 emails/jour gratuitement, pour toujours.

### Q : Puis-je envoyer de vraies pièces jointes ?
**R :** Cette version affiche uniquement les noms de fichiers. Pour envoyer de vraies pièces jointes, vous pouvez étendre la DLL.

### Q : Mes emails vont dans le spam, pourquoi ?
**R :** C'est normal pour les nouveaux comptes SendGrid. Solutions :
- Demandez aux destinataires de marquer comme "Non spam"
- Validez votre domaine dans SendGrid
- Envoyez régulièrement pour construire votre réputation

### Q : Puis-je personnaliser les couleurs ?
**R :** Les couleurs sont fixes :
- Info : Bleu #2196F3
- Erreur : Rouge #f44336
- Urgence : Orange #ff9800

### Q : Comment envoyer du HTML personnalisé ?
**R :** Utilisez du HTML dans le paramètre `message` :
```vb
Dim message = "<h2>Titre</h2><p>Texte avec <strong>gras</strong></p>"
```

### Q : Compatible avec quels projets ?
**R :** La DLL fonctionne avec :
- Console Applications
- Windows Forms (WinForms)
- WPF
- ASP.NET
- Services Windows
- Tous projets VB.NET / .NET Framework 4.8+

---

## 🎨 Types d'Emails Disponibles

| Type | Valeur Enum | Couleur | Utilisation |
|------|-------------|---------|-------------|
| 🔵 **Info** | `TypeEmail.Info` | Bleu #2196F3 | Notifications informatives |
| 🔴 **Erreur** | `TypeEmail.Erreur` | Rouge #f44336 | Alertes d'erreurs |
| 🟠 **Urgence** | `TypeEmail.Urgence` | Orange #ff9800 | Messages urgents |

---

## 📚 Exemples de Code

Consultez les fichiers suivants pour plus d'exemples :
- � **[Exemples Complets](Documentation/ExempleUtilisation.vb)** - 10 exemples détaillés
- 🧪 **[Tests Fonctionnels](Tests/TestEmail.vb)** - 8 tests prêts à l'emploi

---

## � API Reference

### Classe `EmailSender`

#### Constructeur
```vb
Public Sub New(apiKey As String, fromEmail As String, fromName As String)
```

**Paramètres :**
- `apiKey` : Clé API SendGrid (format: `SG.xxxxx...`)
- `fromEmail` : Adresse email de l'expéditeur (doit être validée dans SendGrid)
- `fromName` : Nom affiché de l'expéditeur

**Exemple :**
```vb
Dim sender As New EmailSender(
    "SG.xxxxxxxxxxxxx",
    "contact@example.com",
    "Mon Entreprise"
)
```

#### Méthode `EnvoyerEmailAsync`
```vb
Public Async Function EnvoyerEmailAsync(
    destinataire As String,
    sujet As String,
    message As String,
    Optional typeEmail As TypeEmail = TypeEmail.Info,
    Optional signature As String = Nothing,
    Optional pieceJointes As List(Of String) = Nothing,
    Optional cc As List(Of String) = Nothing,
    Optional cci As List(Of String) = Nothing
) As Task(Of Boolean)
```

**Paramètres :**

| Paramètre | Type | Obligatoire | Description |
|-----------|------|-------------|-------------|
| `destinataire` | `String` | ✅ Oui | Adresse email du destinataire principal |
| `sujet` | `String` | ✅ Oui | Sujet de l'email |
| `message` | `String` | ✅ Oui | Contenu HTML du message |
| `typeEmail` | `TypeEmail` | ❌ Non | `Info` (défaut), `Erreur`, ou `Urgence` |
| `signature` | `String` | ❌ Non | Signature personnalisée en bas de l'email |
| `pieceJointes` | `List(Of String)` | ❌ Non | Liste des noms de fichiers à afficher |
| `cc` | `List(Of String)` | ❌ Non | Liste d'emails en copie carbone |
| `cci` | `List(Of String)` | ❌ Non | Liste d'emails en copie cachée |

**Retour :**
- `Task(Of Boolean)` : `True` si envoyé avec succès, `False` sinon

### Enum `TypeEmail`
```vb
Public Enum TypeEmail
    Info = 0      ' Email informatif (Bleu)
    Erreur = 1    ' Email d'erreur (Rouge)
    Urgence = 2   ' Email urgent (Orange)
End Enum
```

---

## 🔑 Configuration SendGrid

### Obtenir votre clé API SendGrid

1. Créez un compte gratuit sur [SendGrid](https://sendgrid.com/) (100 emails/jour gratuits)
2. Allez dans **Settings → API Keys**
3. Cliquez sur **Create API Key**
4. Donnez-lui un nom et sélectionnez **Full Access**
5. Copiez la clé API générée

### Valider votre adresse email

1. Allez dans **Settings → Sender Authentication**
2. Cliquez sur **Verify a Single Sender**
3. Remplissez le formulaire avec vos informations
4. Vérifiez votre email

---

## � Structure du Projet

```
SenderEmail/
├── README.md                       # Ce fichier - Documentation complète
├── EmailSender.vb                  # Code source principal (350+ lignes)
├── EmailSenderDLL.vbproj           # Fichier projet VB.NET
├── .gitignore                      # Configuration Git
│
├── bin/Release/net48/              # 🎁 DLL COMPILÉE (Prête à utiliser)
│   ├── EmailSenderDLL.dll         # DLL principale (27 KB)
│   ├── Newtonsoft.Json.dll        # Dépendance JSON (695 KB)
│   └── EmailSenderDLL.xml         # Documentation IntelliSense (2.7 KB)
│
├── Documentation/
│   └── ExempleUtilisation.vb      # 10 exemples de code complets
│
└── Tests/
    ├── TestEmail.vb                # 8 tests fonctionnels
    └── TestEmail.vbproj           # Projet de test
```

---

## 📋 Prérequis

- ✅ .NET Framework 4.8 ou supérieur
- ✅ Visual Studio 2019/2022 (ou compatible)
- ✅ Compte SendGrid (gratuit - 100 emails/jour)
- ✅ Projet VB.NET

---

## � Statistiques

| Métrique | Valeur |
|----------|--------|
| **Lignes de code** | 350+ |
| **Taille DLL** | 27 KB |
| **Dépendances** | 1 (Newtonsoft.Json) |
| **Types d'emails** | 3 (Info, Erreur, Urgence) |
| **Exemples fournis** | 10 |
| **Tests inclus** | 8 |
| **Compilation** | ✅ Sans erreurs |

---

## 🛠️ Support et Contact

**Développeur :** Mohamed Ben El Khouja  
**Société :** Tech Dev DAAM  
**Email :** ***REMOVED***

### Besoin d'aide ?
- 📧 Contactez-nous par email
- 💡 Consultez `Documentation/ExempleUtilisation.vb` pour 10 exemples
- 🧪 Testez avec `Tests/TestEmail.vb`

---

## 📝 Changelog

### Version 1.0.0 (31 octobre 2025)
- ✨ Première version stable
- ✅ 3 templates HTML (Info, Erreur, Urgence)
- ✅ Support CC et BCC
- ✅ Signatures personnalisables
- ✅ Affichage de pièces jointes
- ✅ Documentation complète
- ✅ 10 exemples de code
- ✅ 8 tests fonctionnels

---

## 🎯 Notes Importantes

1. **Asynchrone** : Utilisez toujours `Await` avec `EnvoyerEmailAsync`
2. **Rate Limiting** : Ajoutez des pauses (`Task.Delay`) entre les envois en masse
3. **Validation** : Validez votre email expéditeur dans SendGrid avant utilisation
4. **HTML** : Le paramètre `message` accepte du HTML complet
5. **Signatures** : Utilisez `vbCrLf` ou `vbLf` pour les retours à la ligne
6. **Sécurité** : Ne commitez jamais votre clé API dans le code source

---

## 📝 Licence

Ce projet est développé par **Tech Dev DAAM**.  
Libre d'utilisation pour vos projets personnels et commerciaux.

---

## � Félicitations !

Vous êtes maintenant prêt à envoyer des emails professionnels en 3 lignes de code ! 🚀

**Développé avec ❤️ par Tech Dev DAAM**
