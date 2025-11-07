# 📦 EmailSenderDLL v1.2.0 - Package de Distribution

## 📋 Contenu du Package

Ce package contient tout ce dont vous avez besoin pour utiliser EmailSenderDLL dans vos projets :

### Fichiers Inclus

- **EmailSenderDLL.dll** (83 KB) - La bibliothèque principale
- **EmailSenderDLL.xml** (3.2 KB) - Documentation IntelliSense
- **Newtonsoft.Json.dll** (695 KB) - Dépendance requise

---

## 🚀 Installation Rapide

### Pour VB.NET / C# (.NET Framework 4.8)

1. **Copiez les DLL** dans votre projet :
   - EmailSenderDLL.dll
   - Newtonsoft.Json.dll

2. **Ajoutez la référence** dans votre projet :
   - Clic droit sur "Références" → "Ajouter une référence"
   - Parcourir → Sélectionner `EmailSenderDLL.dll`
   - Parcourir → Sélectionner `Newtonsoft.Json.dll`

3. **Copiez EmailSenderDLL.xml** (optionnel) :
   - Placez-le dans le même dossier que la DLL
   - Active IntelliSense pour la documentation automatique

---

## 📧 Utilisation - Exemple Minimal

### VB.NET

```vb
Imports EmailSenderDLL

' Configuration
Dim apiKey = "VOTRE_CLE_API_SENDGRID"
Dim fromEmail = "votre@email.com"
Dim fromName = "Votre Nom"

' Créer l'instance
Dim sender As New EmailSender(apiKey, fromEmail, fromName)

' Envoyer un email
Await sender.EnvoyerEmailAsync(
    "destinataire@example.com",
    "Sujet de l'email",
    "Corps du message",
    TypeEmail.Info
)
```

### C#

```csharp
using EmailSenderDLL;

// Configuration
var apiKey = "VOTRE_CLE_API_SENDGRID";
var fromEmail = "votre@email.com";
var fromName = "Votre Nom";

// Créer l'instance
var sender = new EmailSender(apiKey, fromEmail, fromName);

// Envoyer un email
await sender.EnvoyerEmailAsync(
    "destinataire@example.com",
    "Sujet de l'email",
    "Corps du message",
    TypeEmail.Info
);
```

---

## 🎨 Types d'Emails Disponibles

| Type | Enum | Couleur | Icône | Usage |
|------|------|---------|-------|-------|
| Information | `TypeEmail.Info` | Bleu #2196F3 | ℹ️ | Infos générales |
| Erreur | `TypeEmail.Erreur` | Rouge #f44336 | ❌ | Erreurs/Échecs |
| Urgence | `TypeEmail.Urgence` | Orange #ff9800 | ⚡ | Urgent |
| Succès | `TypeEmail.Succes` | Vert #4caf50 | ✅ | Confirmations |
| Alerte | `TypeEmail.Alerte` | Rouge foncé #b71c1c | 🚨 | Incidents critiques |
| Avertissement | `TypeEmail.Avertissement` | Jaune #f57f17 | ⚠️ | Actions requises |
| Notification | `TypeEmail.Notification` | Violet #7b1fa2 | 🔔 | Notifications |

---

## 📎 Fonctionnalités Avancées

### Avec Signature

```vb
Dim signature = "Jean Dupont" & vbCrLf &
                "Développeur Senior" & vbCrLf &
                "📧 jean@example.com"

Await sender.EnvoyerEmailAsync(
    "destinataire@example.com",
    "Sujet",
    "Message",
    TypeEmail.Info,
    signature
)
```

### Avec Pièces Jointes Réelles

```vb
Dim fichiers As New List(Of String) From {
    "C:\Documents\rapport.pdf",
    "C:\Documents\facture.xlsx"
}

Await sender.EnvoyerEmailAsync(
    "destinataire@example.com",
    "Sujet",
    "Message",
    TypeEmail.Info,
    Nothing,        ' Pas de signature
    Nothing,        ' Pas de liste de noms de fichiers
    fichiers        ' Fichiers réels à attacher
)
```

### Avec CC et BCC

```vb
Dim cc As New List(Of String) From {"cc1@example.com", "cc2@example.com"}
Dim bcc As New List(Of String) From {"bcc@example.com"}

Await sender.EnvoyerEmailAsync(
    "destinataire@example.com",
    "Sujet",
    "Message",
    TypeEmail.Info,
    Nothing,        ' signature
    Nothing,        ' pieceJointes (noms)
    Nothing,        ' fichiersAttaches (fichiers réels)
    cc,             ' CC
    bcc             ' BCC
)
```

---

## ⚙️ Configuration SendGrid

### 1. Créer un compte SendGrid

Visitez : https://sendgrid.com/

### 2. Générer une clé API

- Dashboard → Settings → API Keys
- Create API Key
- Sélectionnez "Full Access" ou "Mail Send" uniquement
- Copiez la clé générée

### 3. Vérifier l'email expéditeur

- Settings → Sender Authentication
- Vérifiez votre email ou domaine

### 4. Limites gratuites

- 100 emails/jour sur le plan gratuit
- Idéal pour développement et petits projets

---

## 🔧 Signature Complète de la Fonction

```vb
Public Async Function EnvoyerEmailAsync(
    destinataire As String,                          ' REQUIS
    sujet As String,                                 ' REQUIS
    message As String,                               ' REQUIS
    Optional typeEmail As TypeEmail = TypeEmail.Info,
    Optional signature As String = Nothing,
    Optional pieceJointes As List(Of String) = Nothing,
    Optional fichiersAttaches As List(Of String) = Nothing,
    Optional cc As List(Of String) = Nothing,
    Optional cci As List(Of String) = Nothing
) As Task(Of Boolean)
```

**Paramètres :**

- `destinataire` : Email du destinataire principal
- `sujet` : Sujet de l'email
- `message` : Corps du message (supporte HTML basique)
- `typeEmail` : Type d'email (Info, Erreur, etc.)
- `signature` : Signature personnalisée (optionnel)
- `pieceJointes` : Liste de noms de fichiers à afficher (optionnel)
- `fichiersAttaches` : Chemins complets des fichiers à attacher (optionnel)
- `cc` : Liste d'emails en copie (optionnel)
- `cci` : Liste d'emails en copie invisible (optionnel)

**Retour :** `Task(Of Boolean)` - `True` si succès, `False` sinon

---

## ⚠️ Prérequis

- **.NET Framework 4.8** ou supérieur
- **Clé API SendGrid** valide
- **Email expéditeur vérifié** dans SendGrid

---

## 📊 Informations Techniques

- **Version :** 1.2.0
- **Framework cible :** .NET Framework 4.8
- **Taille DLL principale :** 83 KB
- **Dépendance :** Newtonsoft.Json 13.0.3
- **API :** SendGrid REST API v3
- **Date de compilation :** 6 novembre 2025

---

## 🐛 Gestion des Erreurs

La fonction retourne `False` en cas d'erreur et affiche un message dans la console.

Pour une gestion d'erreurs personnalisée :

```vb
Try
    Dim resultat = Await sender.EnvoyerEmailAsync(...)
    If resultat Then
        Console.WriteLine("Email envoyé avec succès !")
    Else
        Console.WriteLine("Échec de l'envoi")
    End If
Catch ex As Exception
    Console.WriteLine("Erreur : " & ex.Message)
End Try
```

---

## 📞 Support

**Développé par :** Tech Dev DAAM  
**Email :** ***REMOVED***  
**Version :** 1.2.0  
**Date :** 6 novembre 2025

---

## 📄 Licence

Ce projet est développé pour un usage interne DAAM.

---

## 🎉 Changelog

### v1.2.0 (6 novembre 2025)
- ✨ Ajout de 3 nouveaux types : Alerte, Avertissement, Notification
- 🎨 Total : 7 types d'emails disponibles

### v1.1.0 (6 novembre 2025)
- ✨ Ajout du type Succès (Vert)
- 📎 Support des pièces jointes réelles (Base64)

### v1.0.0 (6 novembre 2025)
- 🎉 Version initiale
- 🎨 3 types : Info, Erreur, Urgence
- 📧 Support CC/BCC
- ✍️ Signatures personnalisables
