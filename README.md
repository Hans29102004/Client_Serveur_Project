# Projet de Réseau Client-Serveur

Ce projet vise à mettre en place un réseau client-serveur permettant à plusieurs clients de communiquer entre eux via un serveur central.

## Fonctionnalités Principales

### Serveur
- 🚀 **Initialisation** : 
  - Demande l'adresse IP du serveur (127.0.0.1 par défaut)
  - Crée un `TcpListener` sur le port **1234**
  - Attend les connexions clientes

### Client
- 💻 **Connexion** :
  - Demande l'adresse du serveur
  - Saisie du pseudo utilisateur
  - Échange de messages en temps réel

### Gestion des Connexions
- 🔄 **Multithread** : Chaque client géré dans un thread séparé
- 📢 **Diffusion** : Messages relayés à tous les clients connectés
- ❌ **Déconnexion** : Notification automatique lorsqu'un client quitte

## Architecture Technique

```csharp
// Exemple de structure clé
TcpListener server = new TcpListener(IPAddress.Parse(ip), 1234);
```

1. **Initialisation Serveur**
   - Création du `TcpListener`
   - Démarrage de l'écoute

2. **Boucle Principale**
   ```mermaid
   graph TD
     A[AcceptClient] --> B[CréerThread]
     B --> C[GérerClient]
   ```

3. **Gestion Client**
   - 📝 Demande de pseudo
   - 📨 Réception/diffusion des messages
   - 🔌 Gestion des déconnexions

## Cas d'Usage

1. Client A se connecte → "PseudoA est connecté"
2. Client B envoie "Hello" → Tous les clients voient "PseudoB: Hello"
3. Client A se déconnecte → "PseudoA est déconnecté"

> **Note** : Solution optimale pour des communications simples entre multiples clients avec une architecture légère.
