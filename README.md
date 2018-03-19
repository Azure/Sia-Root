[![LICENSE](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/Azure/Sia-Root/blob/master/LICENSE)
[![Build Status](https://travis-ci.org/Azure/Sia-Root.svg?branch=master)](https://travis-ci.org/Azure/Sia-Root)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](https://github.com/Azure/Sia-Root/blob/master/HOWTOCONTRIBUTE.md)

# SIA - SRE Incident Assistant

SIA is a new incident management tool that reads from event sources and recommends courses of action that help mitigate incidents quickly.  SIA can read from nearly any event stream or ticketing system, and works with many live site response models. 

## The problem
Software systems are only as effective as they are reliable. As online services grow larger and more complex, the potential complexity of failure modes increases as well. Measures that reduce chances of catastrophic failure (such as redundancy, automated mitigation, and throttling/retry logic) are very effective at preventing outages caused by simple bugs and hardware failures, but cannot (yet) adjust to prevent types of problems that weren't anticipated when those measures were designed. As organizations automate away the need for humans to intervene in cases of simple failures, human incident responders are left to deal with the most complex and pervasive outages. Existing tools are sufficient for most incidents, but fall short in critical situations:
*  [Grey failures](https://www.microsoft.com/en-us/research/wp-content/uploads/2017/06/paper-1.pdf), capacity tipping points, and other cases where multiple systems interact in unanticipated ways to produce problems without a known path to mitigation, especially when changes to code or configuration may result in more impact to users than the problem itself.
* Situations where multiple teams are simultaneously investigating outages, some (but not all) of which share a root cause.
* Long-running issues that require coordination between multiple teams and handoff between shifts within each team over the course of several days or weeks.
* Major security issues such as [Heartbleed](https://en.wikipedia.org/wiki/Heartbleed) and [WannaCry](https://en.wikipedia.org/wiki/WannaCry_ransomware_attack) that require immediate updates across significant portions of an organization's infrastructure.
* Disasters that cause extended or permanent loss of significant physical capacity, including undersea fiber cuts and loss of data center buildings.

## The solution
The SRE Incident Assistant (SIA) is designed to facilitate coordination, communication, and mitigation activities in 'worst case scenario' outages while gathering data for use in postmortem and process improvement.

## Service Architecture Overview
![SIA service architecture](SIA-Architecture.png)

Definitions: 

| Term | Definition | Repo |
| ---- | ---------- | ---- |
| **SIA Root** | Domain objects that make communication between the Gateway and Services easier | [https://github.com/Azure/Sia-Root](https://github.com/Azure/Sia-Root) |
| **Incident Management Data Source** | A ticketing system | 
| **ICM Connector** | Connects an event source to the SIA Gateway | [https://github.com/Azure/Sia-Gateway/tree/master/src/Sia.Connectors.Tickets](https://github.com/Azure/Sia-Gateway/tree/master/src/Sia.Connectors.Tickets) (part of the Sia-Gateway project)|
| **SIA Gateway** | Web API service that receives events, sends events to the Event UI, and to other services | [https://github.com/Azure/Sia-Gateway](https://github.com/Azure/Sia-Gateway) |
| **Services** | Third party services receive events and respond (e.g. with suggested courses of action) based on their internal business logic. Use the Domain objects defined in [Sia-Root](https://github.com/Azure/Sia-Root) | You create services |
| **Event UI** | User interface for SIA | [https://github.com/Azure/Sia-EventUI](https://github.com/Azure/Sia-EventUI) |

# Quick Start
Get up and running with a local resources.

* [Install prerequisites](https://github.com/Azure/Sia-Root/blob/master/HOWTOCONTRIBUTE.md#installing-prerequisites)
  * For Windows Users, the PowerShell script (`installEventUI.ps1`) can help installing the prerequisites, as well as cloning the UI repos.
* Clone the repos:
  * **Gateway:** [https://github.com/Azure/Sia-Gateway](https://github.com/Azure/Sia-Gateway) (Make sure to initalize the submodules)
  ```
  git clone https://github.com/Azure/Sia-Gateway
  git submodule init
  git submodule update
  ```    
  * **UI:** git clone [https://github.com/Azure/Sia-EventUi](https://github.com/Azure/Sia-EventUi)
  ```
  git clone https://github.com/Azure/Sia-EventUi
  ```

* Update the [configurations](https://github.com/Azure/Sia-Root/blob/master/HOWTOCONTRIBUTE.md#development-workflow)
* Launch
  * Additional services
  * Gateway (Follow instructions at [https://github.com/Azure/Sia-Gateway](https://github.com/Azure/Sia-Gateway))
  * Event UI (Follow instructions at [https://github.com/Azure/Sia-EventUI](https://github.com/Azure/Sia-EventUI)) (`npm start` for local development)

* Open [http://localhost:3000](http://localhost:3000) in your browser, and voilà
  
## Contributing

If you are interested in fixing issues and contributing directly to the code base, please see the document How to Contribute, which covers the following:
* [Build and Run from Source](https://github.com/Azure/Sia-Root/blob/master/HOWTOCONTRIBUTE.md#build)
* [The development workflow, including debugging and running tests](https://github.com/Azure/Sia-Root/blob/master/HOWTOCONTRIBUTE.md#debugging)
* [Coding guidelines](https://github.com/Azure/Sia-Root/blob/master/HOWTOCONTRIBUTE.md#work-branches)
* [Submitting pull requests](https://github.com/Azure/Sia-Root/blob/master/HOWTOCONTRIBUTE.md#pull-requests)

Please see also our [Code of Conduct](https://github.com/Azure/Sia-Root/blob/master/CODEOFCONDUCT.md).

## Feedback
* Request a new feature on [GitHub](CONTRIBUTING.md)
* Vote for [popular feature requests](https://github.com/Azure/sia-root/issues?q=is%3Aopen+is%3Aissue+label%3Afeature-request+sort%3Areactions-%2B1-desc).
* File a bug in [GitHub Issues](https://github.com/Azure/sia-root/issues).

## License
Copyright (c) Microsoft Corporation. All rights reserved.
Licensed under the [MIT](https://github.com/Microsoft/vscode/blob/master/LICENSE.txt) License.
