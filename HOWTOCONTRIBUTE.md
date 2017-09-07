# Contributing to Code
There are many ways to contribute to the code project: logging bugs, submitting pull requests, reporting issues, and creating suggestions.

After cloning and building the repo, check out the issues list. 
Issues labeled beginner are good candidates to pick up if you are in the code for the first time.

# Installing Prerequisites
You'll need…
* [NodeJs](https://nodejs.org/en/download/)
* [Visual Studio 2017 or VSCode](https://www.visualstudio.com/downloads/) 
* [.Net Core SDK](https://www.microsoft.com/net/download/core)
* [An Azure Subscription](https://azure.microsoft.com/en-us/pricing/purchase-options/) with:
    * [A Key Vault instance](https://docs.microsoft.com/en-us/azure/key-vault/key-vault-get-started)
    * [An Application Insights instance](https://docs.microsoft.com/en-us/azure/application-insights/app-insights-create-new-resource)
    * [2 Azure Active Directory App Registrations](https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-integrating-applications), 1 for the Gateway and 1 for EventUI.
    * [2 Azure App Services](https://docs.microsoft.com/en-us/azure/app-service-web/), 1 for the Gateway and 1 for EventUI. This is not needed for development if you run on a local machine.

# Development Workflow
## Configurations

## Build
Launch Gateway from VS2017 or VS Code
Start UI with npm: npm start
Open http://localhost:3000 in your browser

## Errors and Warnings

## Debugging
* Gateway
  * Visual Studio 2017 C# debugging.
* EventUI
  * Browser console
  * [React Developer Tools](https://chrome.google.com/webstore/detail/react-developer-tools/fmkadmapgofadopljbjfkapdkoienihi)
  * [Redux Developer Tools](https://chrome.google.com/webstore/detail/redux-devtools/lmhkpmbekcpmknklioeibfkpmmfibljd)
  
## Unit Testing
__Repository__ | __Test framework(s)__ | __How to run tests__ | __Location of test code__
-|-|-|-
Gateway | MSTest in Visual Studio 2017 | See [Test Explorer documentation](https://docs.microsoft.com/en-us/visualstudio/test/run-unit-tests-with-test-explorer#BKMK_Run_tests_in_Test_Explorer) for information on running tests with text explorer in Visual Studio 2017 | Test code projects are in test directory
EventUI | Using Webpack to transpile a test bundle and run Mocha tests | Run 'npm test' using npm command prompt from project root directory | Test source files are in test directory; transpiled test code is run from temp/testBundle.js

# Work Branches
Even if you have push rights on the Azure/SIA repository, you should create a personal fork and create feature branches there when you need them. This keeps the main repository clean and your personal workflow cruft out of sight.

# Pull Requests
Before we can accept a pull request from you, you'll need to sign a [Contributor License Agreement (CLA)](https://github.com/Microsoft/vscode/wiki/Contributor-License-Agreement). It is an automated process and you only need to do it once.
To enable us to quickly review and accept your pull requests, always create one pull request per issue and [link the issue in the pull request](https://github.com/blog/957-introducing-issue-mentions).
Never merge multiple requests in one unless they have the same root cause. Be sure to follow our [Coding Guidelines](https://github.com/Microsoft/vscode/wiki/Coding-Guidelines) and keep code changes as small as possible. Avoid pure formatting changes to code that has not been modified otherwise. Pull requests should contain tests whenever possible.

# Where to Contribute
Check out [the full issues list](https://github.com/Azure/Sia-Root/issues) for a list of all potential areas for contributions. Note that just because an issue exists in the repository does not mean we will accept a contribution to the core editor for it. There are several reasons we may not accepts a pull request.

To improve the chances to get a pull request merged you should select an issue that is labelled with the up-for-grabs label. If the issue you want to work on is not labelled with help-wanted or bug, you can start a conversation with the issue owner asking whether an external contribution will be considered.

# Suggestions
We're also interested in your feedback for the future of Sia. You can submit a suggestion or feature request through the issue tracker. To make this process more effective, we're asking that these include more information to help define them more clearly.

# Discussion Etiquette
In order to keep the conversation clear and transparent, please limit discussion to English and keep things on topic with the issue. Be considerate to others and try to be courteous and professional at all times.
