# 🔐 Configuration Sécurisée - EmailSenderDLL

## ⚠️ ACTION IMMÉDIATE REQUISE

### 1. Révoquer l'ancienne clé API SendGrid

**IMPORTANT:** Une clé API a été exposée dans le code source. Vous DEVEZ la révoquer immédiatement :

1. Connectez-vous à **SendGrid Dashboard** : https://app.sendgrid.com/
2. Allez dans **Settings** → **API Keys**
3. Trouvez la clé commençant par `SG.JGgyDSNkQSOy...`
4. Cliquez sur **Delete** pour la révoquer
5. Créez une **nouvelle clé API** avec les permissions appropriées

### 2. Créer votre fichier .env

```bash
# Depuis la racine du projet
cp .env.example .env
```

Éditez le fichier `.env` et remplacez par vos **vraies valeurs** :

```ini
# SendGrid API Configuration
SENDGRID_API_KEY=SG.votre_nouvelle_cle_api_ici
SENDGRID_FROM_EMAIL=votre-email@domaine.com
SENDGRID_FROM_NAME=Votre Nom ou Entreprise

# Test Email Configuration
TEST_TO_EMAIL=destinataire-test@domaine.com
```

### 3. Vérifier que .env est ignoré par Git

```bash
# Le fichier .env ne doit JAMAIS être committé
git status

# Si .env apparaît, c'est un problème !
# Vérifiez que .gitignore contient bien:
# .env
# *.env
# !.env.example
```

## 📚 Utilisation dans votre code

### Ancienne méthode (DEPRECATED - Non sécurisée)
```vb
Private Const API_KEY As String = "SG.ma_cle_en_dur"  ' ❌ NE PLUS FAIRE ÇA
```

### Nouvelle méthode (RECOMMANDÉE - Sécurisée)
```vb
Imports EmailSenderDLL

Module MonModule
    Private ReadOnly API_KEY As String
    Private ReadOnly FROM_EMAIL As String
    
    Sub New()
        ' Charge automatiquement depuis .env
        EnvConfig.LoadEnvFile()
        
        API_KEY = EnvConfig.GetRequired("SENDGRID_API_KEY")
        FROM_EMAIL = EnvConfig.GetRequired("SENDGRID_FROM_EMAIL")
    End Sub
    
    Sub Main()
        Dim sender As New EmailSender(API_KEY, FROM_EMAIL, "Mon Nom")
        ' ... votre code ...
    End Sub
End Module
```

## 🔒 Bonnes pratiques de sécurité

### ✅ À FAIRE
- Utiliser des variables d'environnement pour tous les secrets
- Créer un fichier `.env` local (jamais committé)
- Fournir un `.env.example` avec des valeurs factices
- Révoquer immédiatement toute clé API exposée
- Utiliser des clés API différentes pour dev/test/prod
- Limiter les permissions des clés API au strict nécessaire

### ❌ À NE JAMAIS FAIRE
- Hardcoder des clés API dans le code source
- Committer le fichier `.env` dans Git
- Partager des clés API par email ou chat
- Utiliser la même clé API en production et développement
- Laisser une clé exposée active "juste quelques heures"

## 🚀 Déploiement en production

### Azure App Service / Function App
```bash
az webapp config appsettings set --name myapp --resource-group mygroup \
  --settings SENDGRID_API_KEY="SG.xxxxx" \
             SENDGRID_FROM_EMAIL="prod@company.com" \
             SENDGRID_FROM_NAME="Production System"
```

### Variables d'environnement système (Windows)
```powershell
[System.Environment]::SetEnvironmentVariable('SENDGRID_API_KEY', 'SG.xxxxx', 'User')
[System.Environment]::SetEnvironmentVariable('SENDGRID_FROM_EMAIL', 'prod@company.com', 'User')
```

### Docker
```dockerfile
ENV SENDGRID_API_KEY="SG.xxxxx"
ENV SENDGRID_FROM_EMAIL="prod@company.com"
ENV SENDGRID_FROM_NAME="Production System"
```

Ou via docker-compose.yml :
```yaml
version: '3.8'
services:
  app:
    env_file:
      - .env  # Charge automatiquement les variables
```

## 📖 Ressources

- [SendGrid API Keys Best Practices](https://docs.sendgrid.com/ui/account-and-settings/api-keys)
- [OWASP Secrets Management Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Secrets_Management_Cheat_Sheet.html)
- [GitHub Secret Scanning](https://docs.github.com/en/code-security/secret-scanning)

## ❓ FAQ

**Q: J'ai déjà exposé ma clé, que faire ?**
R: Révoquez-la IMMÉDIATEMENT sur SendGrid Dashboard et créez-en une nouvelle.

**Q: Puis-je partager mon fichier .env avec mon équipe ?**
R: NON. Chaque développeur doit créer son propre .env avec ses credentials. Partagez seulement .env.example.

**Q: Comment tester en CI/CD ?**
R: Utilisez des secrets GitHub Actions, Azure DevOps Variables, ou autre système de secrets management de votre plateforme CI/CD.

**Q: EnvConfig ne trouve pas mon .env**
R: Assurez-vous que le fichier .env est dans la racine du projet, ou spécifiez le chemin complet dans `LoadEnvFile("/chemin/vers/.env")`.
