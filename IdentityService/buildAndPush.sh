#bin/sh
docker build -t cnazk/naabi-identity-service -f Dockerfile ..
docker push cnazk/naabi-identity-service
kubectl rollout restart deployment naabi-identity-service