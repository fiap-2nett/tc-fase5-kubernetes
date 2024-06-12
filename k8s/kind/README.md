## Passo 1: Instalar Dependências
Primeiro, você precisa instalar as ferramentas necessárias:

- Docker
- Kind
- kubectl
- helm (para gerenciar charts do Kubernetes)

## Passo 2: Criar o Cluster Kubernetes com Kind

```bash
kind create cluster --config k8s/kind/kind-config.yaml --name helpdesk
```

## Passo 3: Instalar o Ingress Nginx

1. Adicionar o repositório Helm do Ingress Nginx:

```bash
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update
```

2. Instalar o Ingress Nginx:

```bash
helm install ingress-nginx ingress-nginx/ingress-nginx
```

Verifique se os pods do Ingress Nginx estão em execução:

```bash
kubectl get pods -n default -l app.kubernetes.io/name=ingress-nginx
```








<!-- # How to install Helm

```bash
$ curl -fsSL -o get_helm.sh https://raw.githubusercontent.com/helm/helm/main/scripts/get-helm-3
$ chmod 700 get_helm.sh
$ ./get_helm.sh
``` -->