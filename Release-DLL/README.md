# 📦 EmailSenderDLL v1.2.1 - Prêt à l'emploi

## ✅ Contenu du Package

Ce dossier contient tout ce dont vous avez besoin pour utiliser EmailSenderDLL dans vos projets :

- **EmailSenderDLL.dll** (87 KB) - DLL principale
- **EmailSenderDLL.xml** (6.9 KB) - Documentation IntelliSense
- **Newtonsoft.Json.dll** (695 KB) - Dépendance requise

## 🚀 Installation Rapide

### 1. Copier les fichiers
Copiez les 3 fichiers dans votre projet.

### 2. Ajouter la référence
Dans votre projet VB.NET/C#, ajoutez une référence à `EmailSenderDLL.dll`.

### 3. Utiliser la DLL

```vb
Imports EmailSenderDLL

' Configuration
Dim sender As New EmailSender(
    "VOTRE_API_KEY_SENDGRID",
    "votre@email.com",
    "Votre Nom"
)

' Envoyer un email
Await sender.EnvoyerEmailAsync(
    "destinataire@example.com",
    "Sujet de l'email",
    "Contenu du message",
    TypeEmail.Info
)
```

## 🎨 7 Types d'Emails Disponibles

| Type | Couleur | Usage |
|------|---------|-------|
| `TypeEmail.Info` | Bleu #2196F3 | Informations générales |
| `TypeEmail.Erreur` | Rouge #f44336 | Erreurs et échecs |
| `TypeEmail.Urgence` | Orange #ff9800 | Situations urgentes |
| `TypeEmail.Succes` | Vert #4caf50 | Confirmations réussies |
| `TypeEmail.Alerte` | Rouge foncé #b71c1c | Incidents critiques |
| `TypeEmail.Avertissement` | Jaune #f57f17 | Avertissements importants |
| `TypeEmail.Notification` | Violet #7b1fa2 | Notifications automatiques |

## 📋 Fonctionnalités

✅ **Templates HTML responsive** - Compatibles Outlook, Gmail, etc.  
✅ **Pièces jointes** - Support fichiers réels (Base64)  
✅ **CC/BCC** - Copies carbone et invisibles  
✅ **Signatures personnalisées** - Ajout automatique  
✅ **API Asynchrone** - Async/Await supporté  
✅ **IntelliSense** - Documentation complète  

## 🔧 Configuration SendGrid

Obtenez votre clé API gratuite sur https://sendgrid.com (100 emails/jour gratuits)

## 📝 Version

**v1.2.1** - 7 novembre 2025
- 7 types d'emails avec headers colorés
- Compatible Outlook (couleurs unies)
- Support pièces jointes réelles
- Texte blanc forcé avec !important

## 📞 Support

Projet : `/Users/mahmoudbenelkhouja/Desktop/Tools/SenderEmail`  
Développé par : **Tech Dev DAAM**
