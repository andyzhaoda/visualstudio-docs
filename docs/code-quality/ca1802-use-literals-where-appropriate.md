---
title: "CA1802: Use Literals Where Appropriate"
ms.date: 11/04/2016
ms.technology: vs-ide-code-analysis
ms.topic: reference
f1_keywords:
  - "UseLiteralsWhereAppropriate"
  - "CA1802"
helpviewer_keywords:
  - "UseLiteralsWhereAppropriate"
  - "CA1802"
ms.assetid: 2515e4cd-9e61-486d-b067-58ba1a743ce4
author: gewarren
ms.author: gewarren
manager: douge
ms.workload:
  - "multiple"
---
# CA1802: Use Literals Where Appropriate
|||
|-|-|
|TypeName|UseLiteralsWhereAppropriate|
|CheckId|CA1802|
|Category|Microsoft.Performance|
|Breaking Change|Non-breaking|

## Cause
 A field is declared `static` and `readonly` (`Shared` and `ReadOnly` in [!INCLUDE[vbprvb](../code-quality/includes/vbprvb_md.md)]), and is initialized with a value that is computable at compile time.

## Rule Description
 The value of a `static``readonly` field is computed at runtime when the static constructor for the declaring type is called. If the `static``readonly` field is initialized when it is declared and a static constructor is not declared explicitly, the compiler emits a static constructor to initialize the field.

 The value of a `const` field is computed at compile time and stored in the metadata, which increases runtime performance when it is compared to a `static``readonly` field.

 Because the value assigned to the targeted field is computable at compile time, change the declaration to a `const` field so that the value is computed at compile time instead of at runtime.

## How to Fix Violations
 To fix a violation of this rule, replace the `static` and `readonly` modifiers with the `const` modifier.

## When to Suppress Warnings
 It is safe to suppress a warning from this rule, or disable the rule, if performance is not of concern.

## Example
 The following example shows a type, `UseReadOnly`, that violates the rule and a type, `UseConstant`, that satisfies the rule.

 [!code-vb[FxCop.Performance.UseLiterals#1](../code-quality/codesnippet/VisualBasic/ca1802-use-literals-where-appropriate_1.vb)]
 [!code-csharp[FxCop.Performance.UseLiterals#1](../code-quality/codesnippet/CSharp/ca1802-use-literals-where-appropriate_1.cs)]