docker build . -t bfecchio/helpdesk-apiservice-api -f src/HelpDesk.ApiService.Api/Dockerfile
docker build . -t bfecchio/helpdesk-apiservice-api:v1 -f src/HelpDesk.ApiService.Api/Dockerfile
docker push bfecchio/helpdesk-apiservice-api --all-tags

docker build . -t bfecchio/helpdesk-appservice-web -f src/HelpDesk.AppService.Web/Dockerfile
docker build . -t bfecchio/helpdesk-appservice-web:v1 -f src/HelpDesk.AppService.Web/Dockerfile
docker push bfecchio/helpdesk-appservice-web --all-tags


kubectl apply -f https://projectcontour.io/quickstart/contour.yaml
kubectl apply -f k8s/deployment.yaml

kubectl port-forward service/helpdesk-log 8081:80
kubectl port-forward service/helpdesk-api 5001:443
kubectl port-forward service/helpdesk-web 5002:443


### Delete all deployments inside the default namespace
kubectl delete deployment --all
