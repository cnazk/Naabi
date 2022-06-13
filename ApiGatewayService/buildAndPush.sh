#bin/sh
docker build -t cnazk/naabi-api-gateway -f Dockerfile ..
docker push cnazk/naabi-api-gateway
kubectl rollout restart deployment naabi-api-gateway