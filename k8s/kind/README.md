## Passo 1: Instalar Dependências
Primeiro, você precisa instalar as ferramentas necessárias:

- Docker
- Kind
- kubectl

## Passo 2: Criar o Cluster Kubernetes com Kind

```bash
kind create cluster --config kind-config.yaml
```
## Passo 3: Instalar o Ingress Nginx

1. Aplicar os recursos YAML do Ingress Nginx: Vamos aplicar os arquivos YAML diretamente para instalar o Ingress Nginx.

```bash
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/main/deploy/static/provider/kind/deploy.yaml
```

2. Verificar se o Ingress Nginx está em execução:

```bash
kubectl get pods -n ingress-nginx
```
