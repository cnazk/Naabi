#bin/sh
docker build -t cnazk/naabi-activities-service -f Dockerfile ..
docker push cnazk/naabi-activities-service
kubectl rollout restart deployment naabi-activities-service
