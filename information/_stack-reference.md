# STACK REFERENCE

This document describes the technical stack and operational assumptions.

Planner and Coder MUST treat this as factual reference.

---

# 1. Repository Structure

api/src     → backend source code
api/test    → backend tests
frontend    → frontend application
deployment  → docker + infra 
docs        → human documentation

---

# 2. Stack

## 2A. Backend (API)

Language: C#

Runtime: .NET 10

Testing: xUnit (assumed)

dotnet commands:

``` bash
dotnet build
dotnet test
``` 

## 2B. Frontend (minimal)

Language: Typescript
Framework: React + vite + next.js
Testing: vitest

React commands:

Create a new project	
``` bash
npm create vite@latest #(then select react from the prompts)
npm create vite@latest my-react-app -- --template react #(Alternative create)	
``` 
Install dependencies
``` bash
npm install or npm i #(inside project directory)
``` 

Start development server
``` bash
npm run dev (usually runs on http://localhost:5173/)
``` 

Build for production
``` bash
npm run build
``` 

Preview production build	npm run preview
``` bash
npm run dev -- --port 4000
``` 
Specify port	
``` bash
npm run dev -- --port 4000
``` 

## 2C. SQL Server

Version: latest developer edition

---

# 3. AI Execution Model

Planner:
- Generates plans and tasks
- Does NOT implement code

Coder:
- Implements tasks
- Must follow rules/ and AllowedWriteTargets

---

# 4. Git Workflow

Base branch: development
Feature branches defined in mini-plan:
feature/*
fix/*
refactor/*

Human performs:
development → main PR

---

# 5. Useful Commands

## Validate current task
utilities/scripts/validate-task.sh

## Validate stage
utilities/scripts/validate-stage.sh

## Get current task
utilities/scripts/current-task.sh

## Start planner
utilities/scripts/start-planner.sh