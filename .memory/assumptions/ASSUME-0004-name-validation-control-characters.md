---
id: ASSUME-0004
title: Unicode Recipient Names and ASCII Control Character Validation
component: business-logic
domain: validation
tags:
- validation
- control-characters
- hello-world
- unicode
status: validated
created: '2026-03-24'
updated: '2026-03-24'
depends_on:
- specs/hello-world/parameterized-greeting-spec.md
supported_by:
- specs/hello-world/proposal.md
---

## Assumption

Client applications supply greeting names in UTF-8 encoding. Valid recipient names can contain international Unicode letters, spaces, hyphens, and apostrophes up to 50 characters. The system must reject ASCII control characters (byte values 0x00 through 0x1F and 0x7F) with HTTP status code 400.

## Context

The parameterized greeting specification mandates rejection of unescaped control characters and limits names to 50 characters. The service operates across global customer segments. The service must accept valid international characters while blocking control characters and protocol injection attempts.

## Resolution

Validated. Automated unit tests and endpoint tests confirmed that the service accepts valid Unicode recipient names. The service rejects inputs exceeding 50 characters or containing ASCII control characters with HTTP 400 Problem Details.
