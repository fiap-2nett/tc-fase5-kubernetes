## Passo 1: Instalar Dependências
Primeiro, você precisa instalar as ferramentas necessárias:

- Docker
- Kind
- kubectl

## Passo 2: Criar o Cluster Kubernetes com Kind

Após as ferramentas instaladas, iremos criar nosso cluster no KIND para provisionamento dos serviços e componentes necessários para a execução do nosso projeto.

O comando a seguir utiliza a CLI do KIND para criar um novo cluster com o nome `k8s-helpdesk` utilizando como parâmetro de configuração o arquivo de manifesto `kind-config.yaml`.

Este arquivo contém as parametrizações básicas para dos nodes do tipo:
- control-plane
- worker

Execute o comando a seguir para criar no novo cluster:

```bash
kind create cluster --config k8s/kind-config.yaml --name k8s-helpdesk
```

## Passo 3: Instalar o Ingress Nginx

Um vez configurado o nosso cluster, precisamos configurar o serviço de Ingress Controller para nosso cluster, este serviço nos possibilitará ter um único ponto de acesso aos nossos container em execução sem a necessidades de configurarmos o redirecionamento de portas `port-forward` para cada um de nossos pods.

Existem diversas opções de serviços de Ingress Controller para Kubernetes, para nosso caso de uso optamos por utilizar o Ingress Nginx.

1. Aplicar os recursos YAML do Ingress Nginx: Vamos aplicar os arquivos YAML diretamente para instalar o Ingress Nginx.

```bash
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/main/deploy/static/provider/kind/deploy.yaml
```

2. Verificar se o Ingress Nginx está em execução:

```bash
kubectl get pods -n ingress-nginx
```

Verifique se os pods do Ingress Nginx estão em execução:

```bash
kubectl get pods -n ingress-nginx
```

## Passo 4: Configurar o Deploy dos Serviços

Após a criação do cluster e configuração do serviço de Ingress, podemos configurar/criar os componentes necessários para o deploy e orquestração do nosso projeto.

Para isso, utilizaremos novamente a CLI `kubectl` para aplicar nossas configurações utilizando como base o arquivo de manifesto `deployment.yaml`.

Este arquivo contém diversas configurações pertinentes ao nosso deploy, estas poderiam estar em arquivos separados, mas com o intuito de facilitar a gestão e centralizar o processo de deploy todas as configurações estão presentes neste arquivo.

As configurações contidas nesse arquivo estão separadas em blocos, cada bloco agrupa as configurações necessárias de cada componente, sendo eles:

1. **pfx-secret.yaml**
    > Cria o secret no k8s para armazenar o certificado SSL utilizado para criptografia de tokens JWT.
2. **tls-secret.yaml**
    > Cria o secret no k8s utilizado pelo Ingress nas configurações de TLS/HTTPS.
3. **helpdesk-db-deployment.yaml**
    > Configura o deploy do container de banco de dados SQL.
4. **helpdesk-log.yaml**
    > Configura o deploy do container do SEQ para ingestão de logs de aplicação.
5. **helpdesk-api-deployment.yaml**
    > Configura o deploy do container da HelpDesk.Api.
6. **helpdesk-web-deployment.yaml**
    > Configura o deploy do container da HelpDesk.Web.
7. **helpdesk-ingress-http.yaml**
    > Configura o serviço de Ingress para as requisições HTTP.
8. **helpdesk-ingress-https.yaml**
    > Configura o serviço de Ingress para as requisições HTTPS.

Execute o comando a seguir para aplicar todas as configurações mencionadas acima:

```bash
kubectl apply -f k8s/kind/deployment.yaml
```

Para monitorar o processamento do deployment e provisionamentos dos services e pods, execute o comando:

```bash
watch 'kubectl get pod,svc,deployment,ingress -A'
```

## Passo 5: Acessando o Serviços

Como estamos executando nosso projeto em um cluster K8S local, não será atribuído dinâmicamente um DNS para acesso público.

Para conseguirmos acessar o nosso ponto de entrada sem a necessidade de ficar obtendo IPs, redirecionamento de portas ou inclusão de DNS nos hosts do nosso sistema, optamos um serviço público `nip.io` que automaticamente resolverá nossa requisição para nosso localhost.

Para acessar os serviços configurados no cluster utilize as seguintes URLs:

- HelpDesk.Logs: http://logs.helpdesk.127.0.0.1.nip.io
- HelpDesk.Web: https://app.helpdesk.127.0.0.1.nip.io
- HelpDesk.Api: https://api.helpdesk.127.0.0.1.nip.io/swagger