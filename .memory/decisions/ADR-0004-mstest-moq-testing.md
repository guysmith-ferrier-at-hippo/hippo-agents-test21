---
id: ADR-0004
title: Standardization on MSTest and Moq Without Fluent Assertion APIs
component: testing
domain: quality-assurance
tags:
- mstest
- moq
- unit-testing
- dotnet-10
status: accepted
created: '2026-03-24'
updated: '2026-03-24'
---

# Standardization on MSTest and Moq Without Fluent Assertion APIs

## Context
Unit testing across the Customer Greeting Service layers requires consistent testing tools, clear assertions, and effective interface isolation without external third-party assertion dependencies.

## Decision
We will standardize on Visual Studio Unit Testing (MSTest) as the test framework and Moq as the mocking framework. Fluent assertion libraries (such as FluentAssertions) will not be used; all test verifications will use native MSTest assertions (`Assert.AreEqual`, `Assert.IsTrue`, `Assert.ThrowsException`, etc.).

## Consequences
- **Positive**: Native test framework compatibility with Visual Studio and Azure DevOps Pipelines test runners, zero external assertion dependency overhead, and straightforward assertions.
- **Negative**: Test assertion syntax is slightly more verbose than fluent chaining.
- **Neutral**: All team members must adhere to standard MSTest assertion idioms.

## Options Considered
- **xUnit with FluentAssertions**: Common in .NET ecosystems, but rejected in favor of the explicit mandate for MSTest and native assertions.
- **NSubstitute**: Alternative mocking tool, rejected in favor of Moq.
