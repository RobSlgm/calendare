# Development

## Build container locally with `dotnet publish`

On Windows use:

```ps1
dotnet publish --os linux --arch x64 /t:PublishContainer /p:ContainerImageTags=`"$(nbgv get-version -v SemVer2)`;$(nbgv get-version -v MajorMinorVersion)`;$(nbgv get-version -v Version)`;latest`" -p ContainerRegistry=registry.slgm.ch
```

The registry credentials need to be in ~/.docker/config.json.

## Build Database Migration

In Data subdirectory

```shell
dotnet ef migrations bundle --self-contained -r linux-x64 --force -o ../artifacts/efbundle
# OR for local execution on windows
dotnet ef migrations bundle --force -o ../artifacts/efbundle.exe
```

## Build container with CI

See .gitea or .github directories for action definition.

## Build Helm chart

```shell
helm registry login registry.slgm.ch
```

While standing in the root directory (parent directory of subdirectory `deploy`) create and push helm package with

```shell
helm package --app-version "$(nbgv get-version -v SemVer2)" --version "$(nbgv get-version -v SemVer2)" .\deploy\
helm push .\calendare-server-$(nbgv get-version -v SemVer2).tgz oci://registry.slgm.ch/helm
helm show values oci://registry.slgm.ch/helm/calendare
```

## Deploy using Helm

Allow access to registry with

```shell
kubectl create secret generic reg-cred --from-file=.dockerconfigjson=harbor-config.json --type=kubernetes.io/dockerconfigjson --namespace calendare --context testlab
```

Use for testing of the chart

```shell
helm template calendare . --namespace calendare -f c:\develop\config.k8s-cluster\k8s.testlab\calendare\values.yaml --kube-context testlab
```

For upgrade or installation while standing in the root source directory

```shell
helm upgrade --install calendare .\deploy\ --namespace calendare -f c:\develop\config.k8s-cluster\k8s.testlab\calendare\values.yaml --kube-context testlab --set image.tag=0.1.0-alpha.417.ge397539497
```

- OR for the current version use

```shell
helm upgrade --install calendare .\deploy\ --namespace calendare -f c:\develop\config.k8s-cluster\k8s.testlab\calendare\values.yaml --kube-context testlab --set image.tag=$(nbgv get-version -v SemVer2)
```

- OR with helm repository -

```shell
helm upgrade --install calendare oci://registry.slgm.ch/helm/calendare --namespace calendare -f c:\develop\config.k8s-cluster\k8s.testlab\calendare\values.yaml --kube-context testlab
```

### Uninstall helm chart

```shell
helm uninstall calendare  --namespace calendare --kube-context testlab
```

## Update OpenAPI specification

Start the server locally (or change the URL to a running instance)

```shell
curl -X GET  https://localhost:5443/openapi/v1/openapi.json >.\openapi.json
```

The client libraries in other repositories need to be updated.

## Development credentials

See [User secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-10.0&tabs=windows) for background information.
