---
id: ADR-0003
title: Hosting on Azure Kubernetes Service (AKS) Deployed via Azure DevOps Pipelines
component: infrastructure
domain: cloud-hosting
tags:
- azure
- aks
- azure-devops
- containers
status: accepted
created: '2026-03-24'
updated: '2026-03-24'
---

# Hosting on Azure Kubernetes Service (AKS) Deployed via Azure DevOps Pipelines

## Context
The Customer Greeting Service must be deployed into enterprise environments (Dev, Test, UAT, and Production) on Microsoft Azure with consistent containerized orchestration and automated CI/CD.

## Decision
We will package the Customer Greeting Service as a containerized .NET 10 microservice and deploy it onto Azure Kubernetes Service (AKS). Automated building, testing, container packaging, and progressive deployment across Dev, Test, UAT, and Production environments will be managed through Azure DevOps Pipelines.

## Consequences
- **Positive**: Standardized deployment artifact (Docker container), environment consistency across the release lifecycle, native Kubernetes scaling, self-healing, and observability within Azure.
- **Negative**: Requires Kubernetes manifest and Helm chart management, cluster resource allocation, and container orchestration overhead.
- **Neutral**: The service design must remain container-friendly and stateless in line with ephemeral repository capabilities.

## Options Considered
- **Azure App Service**: Simpler hosting model, but does not align with the enterprise Kubernetes platform standard.
- **Azure Container Apps**: Serverless container option, but AKS was selected for enterprise multi-environment cluster consistency.
