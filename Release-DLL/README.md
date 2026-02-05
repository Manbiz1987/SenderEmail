# EmailSenderDLL v2.0.0

DLL VB.NET flexible pour l'envoi d'emails via **Resend API** ou **SMTP (Gmail, Outlook, etc.)** avec 7 templates HTML personnalisés et configuration 100% paramétrable.

## 🎉 Nouveautés v2.0.0

- ➕ **Méthode SMTP** : Support Gmail, Outlook, Office 365, etc.
- ➕ **4 nouveaux types** d'emails : Succès, Alerte, Avertissement, Notification
- ➕ **Constructeur simplifié** : Chargement automatique depuis .env
- ➕ **Configuration paramétrable** : Tout contrôlable via .env
- ➕ **Pièces jointes réelles** : Attachement de fichiers pour SMTP
- ➕ **Priorités d'emails** : Automatiques selon le type (Haute/Normale)
- ➕ **0 warnings** : Code optimisé sans avertissements de compilation

---

## 🚀 Fonctionnalités

### Méthodes d'envoi
- ✅ **Méthode 1** : Resend REST API
- ✅ **Méthode 2** : SMTP (Gmail, Outlook, Office 365, etc.) - **NOUVEAU**

### 7 Types de templates HTML professionnels
- 🔵 **Info** (Bleu #2196F3) - Informations générales
- 🔴 **Erreur** (Rouge #f44336) - Notifications d'erreurs
- 🟠 **Urgence** (Orange #ff9800) - Actions urgentes (priorité haute)
- 🟢 **Succès** (Vert #4caf50) - Confirmations de succès
- 🔴 **Alerte** (Rouge foncé #b71c1c) - Alertes critiques (priorité haute)
- 🟡 **Avertissement** (Jaune #f57f17) - Avertissements
- 🟣 **Notification** (Violet #7b1fa2) - Notifications générales

### Fonctionnalités avancées
- ✅ Configuration 100% paramétrable via fichier `.env`
- ✅ Support CC (copie carbone) et BCC (copie carbone invisible)
- ✅ Signatures personnalisables (HTML)
- ✅ Pièces jointes réelles (fichiers attachés)
- ✅ Affichage visuel des pièces jointes dans le template
- ✅ Templates HTML responsive
- ✅ Priorités d'emails automatiques (Haute pour Urgence/Alerte)
- ✅ Constructeur simplifié avec chargement automatique de la config

---

## 📦 Installation

1. **Téléchargez les DLL** depuis le dossier `Release-DLL/` :
   - `EmailSenderDLL.dll` (DLL principale - 94 KB)
   - `Newtonsoft.Json.dll` (dépendance requise - 695 KB)
   - `EmailSenderDLL.xml` (Documentation IntelliSense)

2. **Ajoutez les références** dans votre projet VB.NET

3. **Copiez le fichier `.env`** dans le dossier de votre exécutable

4. **Configurez vos credentials** dans le fichier `.env`

---

## ⚙️ Configuration

### Fichier .env

Créez un fichier `.env` à la racine de votre projet avec toutes les configurations :

```env
# ==================================================
# MÉTHODE 1 : Resend API
# ==================================================
RESEND_API_KEY=votre_cle_api_resend
RESEND_FROM_EMAIL=expediteur@votredomaine.com
RESEND_FROM_NAME=Votre Nom

# ==================================================
# MÉTHODE 2 : SMTP Gmail (NOUVEAU)
# ==================================================
SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SMTP_USERNAME=votre.email@gmail.com
SMTP_PASSWORD=mot_de_passe_application_gmail
SMTP_FROM_EMAIL=votre.email@gmail.com
SMTP_FROM_NAME=Votre Nom
SMTP_ENABLE_SSL=True

# ==================================================
# Configuration par défaut des emails (optionnel)
# ==================================================
EMAIL_TO=destinataire@email.com
EMAIL_CC=cc1@email.com;cc2@email.com
EMAIL_BCC=bcc1@email.com;bcc2@email.com
EMAIL_SUBJECT=Sujet par défaut
EMAIL_TYPE=Info
EMAIL_SIGNATURE=Cordialement,<br>Votre Nom
EMAIL_ATTACHMENTS=C:/fichier1.pdf;C:/fichier2.xlsx

# Test
TEST_TO_EMAIL=test@email.com
```

### ⚠️ Important pour Gmail

Pour utiliser Gmail SMTP, vous devez créer un **mot de passe d'application** :

1. Allez sur https://myaccount.google.com/apppasswords
2. Activez la validation en 2 étapes si nécessaire
3. Créez un mot de passe d'application (sélectionnez "Mail" ou "Autre")
4. Gmail générera un mot de passe de 16 caractères (ex: `abcd efgh ijkl mnop`)
5. Utilisez ce mot de passe dans `SMTP_PASSWORD` (pas votre mot de passe Gmail habituel)

---

## 💻 Utilisation

### Méthode 1 : Resend API

```vb
Imports EmailSenderDLL

' Chargement de la configuration
EnvConfig.LoadEnvFile()

' Création du sender Resend
Dim sender As New EmailSender(
    apiKey:=Environment.GetEnvironmentVariable("RESEND_API_KEY"),
    fromEmail:=Environment.GetEnvironmentVariable("RESEND_FROM_EMAIL"),
    fromName:=Environment.GetEnvironmentVariable("RESEND_FROM_NAME")
)

' Envoi d'un email
Dim resultat = sender.EnvoyerEmailAsync(
    destinataire:="destinataire@email.com",
    sujet:="Mon sujet",
    message:="Mon message <strong>HTML</strong>",
    typeEmail:=TypeEmail.Info
).Result

If resultat Then
    Console.WriteLine("Email envoyé avec succès !")
End If
```

### Méthode 2 : SMTP Gmail - Ultra-Simplifié ⭐

#### Option A : Configuration automatique depuis .env (Recommandé)

```vb
Imports EmailSenderDLL

' ✨ ULTRA-SIMPLIFIÉ : Tout chargé automatiquement depuis .env
Dim sender As New EmailSenderSMTP()

' Envoi avec configuration par défaut (tout depuis .env)
sender.EnvoyerEmailAsync(
    message:="Mon message <strong>HTML</strong>"
).Wait()

' Envoi avec override de certains paramètres
sender.EnvoyerEmailAsync(
    message:="Message urgent",
    destinataire:="autre@email.com",
    sujet:="Email urgent",
    typeEmail:=TypeEmail.Urgence
).Wait()
```

#### Option B : Configuration manuelle

```vb
Imports EmailSenderDLL

' Configuration manuelle complète
Dim sender As New EmailSenderSMTP(
    smtpHost:="smtp.gmail.com",
    smtpPort:=587,
    username:="votre.email@gmail.com",
    password:="mot_de_passe_application",
    fromEmail:="votre.email@gmail.com",
    fromName:="Votre Nom",
    enableSsl:=True
)

' Envoi
sender.EnvoyerEmailCompletAsync(
    destinataire:="destinataire@email.com",
    sujet:="Mon sujet",
    message:="Mon message",
    typeEmail:=TypeEmail.Succes
).Wait()
```

### Exemple avec tous les types d'emails

```vb
Dim sender As New EmailSenderSMTP()

' Email Info (Bleu)
sender.EnvoyerEmailAsync(
    message:="Information générale",
    typeEmail:=TypeEmail.Info
).Wait()

' Email Erreur (Rouge)
sender.EnvoyerEmailAsync(
    message:="Une erreur s'est produite",
    typeEmail:=TypeEmail.Erreur
).Wait()

' Email Urgence (Orange - Priorité Haute)
sender.EnvoyerEmailAsync(
    message:="Action immédiate requise",
    typeEmail:=TypeEmail.Urgence
).Wait()

' Email Succès (Vert)
sender.EnvoyerEmailAsync(
    message:="Opération réussie !",
    typeEmail:=TypeEmail.Succes
).Wait()

' Email Alerte (Rouge foncé - Priorité Haute)
sender.EnvoyerEmailAsync(
    message:="Alerte de sécurité",
    typeEmail:=TypeEmail.Alerte
).Wait()

' Email Avertissement (Jaune)
sender.EnvoyerEmailAsync(
    message:="Attention requise",
    typeEmail:=TypeEmail.Avertissement
).Wait()

' Email Notification (Violet)
sender.EnvoyerEmailAsync(
    message:="Nouvelle notification",
    typeEmail:=TypeEmail.Notification
).Wait()
```

### Exemple avec pièces jointes et CC/BCC

```vb
' SMTP avec pièces jointes réelles
Dim sender As New EmailSenderSMTP()

Dim resultat = sender.EnvoyerEmailAsync(
    message:="<h2>Rapport mensuel</h2><p>Veuillez trouver ci-joint le rapport.</p>",
    destinataire:="destinataire@email.com",
    sujet:="Rapport mensuel - Janvier 2026",
    typeEmail:=TypeEmail.Info,
    signature:="Cordialement,<br>Équipe IT<br>MBTI Consult",
    fichiersAttaches:=New List(Of String) From {
        "C:\Rapports\rapport_janvier.pdf",
        "C:\Rapports\graphiques.xlsx"
    },
    cc:=New List(Of String) From {"manager@entreprise.com"},
    cci:=New List(Of String) From {"archive@entreprise.com"}
).Wait()
```

---

## 📧 Types d'emails disponibles

| Type | Couleur | Usage | Enum | Priorité |
|------|---------|-------|------|----------|
| Info | 🔵 Bleu (#2196F3) | Informations générales | `TypeEmail.Info` | Normale |
| Erreur | 🔴 Rouge (#f44336) | Notifications d'erreurs | `TypeEmail.Erreur` | Normale |
| Urgence | 🟠 Orange (#ff9800) | Actions urgentes | `TypeEmail.Urgence` | **Haute** |
| Succès | 🟢 Vert (#4caf50) | Confirmations de succès | `TypeEmail.Succes` | Normale |
| Alerte | 🔴 Rouge foncé (#b71c1c) | Alertes critiques | `TypeEmail.Alerte` | **Haute** |
| Avertissement | 🟡 Jaune (#f57f17) | Avertissements | `TypeEmail.Avertissement` | Normale |
| Notification | 🟣 Violet (#7b1fa2) | Notifications générales | `TypeEmail.Notification` | Normale |

---

## 🎯 Comparaison des méthodes

| Fonctionnalité | Méthode 1 (Resend) | Méthode 2 (SMTP) |
|----------------|-------------------|------------------|
| Templates HTML | ✅ | ✅ |
| 7 types d'emails | ✅ | ✅ |
| CC/BCC | ✅ | ✅ |
| Signatures | ✅ | ✅ |
| Pièces jointes réelles | ❌ | ✅ |
| Priorités d'emails | ❌ | ✅ |
| Gratuit | ❌ (API payante) | ✅ (Gmail gratuit) |
| Configuration .env | ✅ | ✅ |
| Constructeur simplifié | ❌ | ✅ |

---

## 🔧 Prérequis

- **.NET Framework 4.8**
- **Newtonsoft.Json** (inclus dans Release-DLL)
- **Pour Méthode 1** : Une clé API Resend valide
- **Pour Méthode 2** : Un compte Gmail (ou autre SMTP) avec mot de passe d'application

---

## 📝 Notes importantes

- ✅ Les templates HTML sont générés dynamiquement (pas de fichiers externes)
- ✅ Support complet des caractères UTF-8
- ✅ Gestion d'erreurs complète avec messages explicites
- ✅ Toutes les opérations sont asynchrones
- ✅ **0 Avertissements, 0 Erreurs** de compilation
- ✅ Configuration 100% paramétrable via .env
- ✅ Support des pièces jointes réelles pour SMTP
- ✅ Priorités d'emails automatiques (Haute/Normale)
- ⚠️ **Gmail nécessite un mot de passe d'application** (pas le mot de passe habituel)

---

## 🧪 Tests

La DLL a été testée avec succès :
- ✅ 7 types d'emails envoyés avec succès
- ✅ Configuration automatique depuis .env
- ✅ Override des paramètres
- ✅ Pièces jointes multiples
- ✅ CC et BCC
- ✅ Signatures personnalisées
- ✅ Priorités d'emails

**Résultat** : 7/7 emails envoyés avec succès ✅

---

## 📄 Licence

Ce projet est une DLL propriétaire développée pour **MBTI Consult**.

---

## 👨‍💻 Auteur

**MBTI Consult**  
Email: mbticonsult@gmail.com

---

## 🆘 Support

Pour toute question ou problème :
1. Vérifiez que le fichier `.env` est bien configuré
2. Pour Gmail, assurez-vous d'utiliser un mot de passe d'application
3. Vérifiez que les DLL sont bien référencées dans votre projet
4. Consultez la documentation IntelliSense (EmailSenderDLL.xml)

---

**Version** : 2.0.0  
**Date** : 27 janvier 2026  
**Compilation** : 0 Avertissements, 0 Erreurs ✅
