## Adapter Pattern Example
### Payment Gateway Adapter
#### A system was designed to work with traditional payment gateways (credit cards, bank transfers).
#### Now it needs to support ethereum payments (more types can be added in future).
- The system expects a standard payment interface
- Crypto APIs are completely different (wallets, hashes, confirmations)
- Core system cannot be changed → you adapt the new payment type into your existing contract


## Builder Pattern Example
### Autonomous Drone Mission Builder
#### In a drone control system, a mission is a complex object composed of multiple parts:
- Navigation plan
- Sensor configuration
- Communication protocol
- Safety constraints
- Payload configuration
#### Different missions require different configurations, for example:
- Surveillance mission
- Delivery mission
#### Instead of building the mission in one complex step it`s splited in many.


## Composite Pattern Example
### Power Grid Control System
#### A national power grid is composed of hierarchical components:
```text
Power Grid
   ├── Region
   │   ├── Substation
   │   │   ├── Transformer
   │   │   └── Transformer
   │   └── Substation
   └── Region
```
#### A control system must be able to ask any element:
- "What is your total power load?"
#### Whether the element is:
- a single transformer (leaf)
- a substation (group)
- a region
- the entire grid


## Facade Pattern Example
### Rocket Launch System
#### A rocket launch involves many specialized subsystems that must be coordinated.
#### In a real architecture typical subsystems involved are:
- Propulsion system
- Guidance & navigation
- Telemetry
- Environmental monitoring
- Range safety
- Ground communication
#### Each subsystem has its own complex procedures and internal logic. 
#### The launch controller should not interact with all of them directly.
#### Instead, a launch facade orchestrates the process.


## Observer Pattern Example
### Concert Ticket Notification System
#### In a ticketing platform:
- Multiple users are interested in the same concert
- Ticket releases happen dynamically (new batches, cancellations, promotions)
- Users should be notified immediately and automatically in order to be able to buy the tickets


## Proxy Pattern Example
### Industrial Robot Safety Proxy
#### In an automated factory, robots perform operations such as:
- Welding
- Cutting
- Drilling
- Moving heavy components

#### However, direct access to the robot controller is dangerous.
#### Commands must pass through a safety layer that verifies conditions such as:
- Emergency stop status
- Operator authorization
- Safety zone clearance
  
#### This safety layer acts as a Proxy.


## Singleton Pattern Example
### Satellite Station Command Center
#### In a satellite control system, all commands sent to a satellite must go 
#### through one command dispatcher to guarantee:
- Command ordering
- Transmission integrity
- Collision avoidance

#### If two dispatchers existed, conflicting commands could be sent simultaneously.


## Strategy Pattern Example
### Adaptive Energy Management Strategy in Electric Vehicles (EV)
#### Modern EVs dynamically adjust energy usage strategies depending on:
- Driving conditions
- Battery state
- User preference
- Navigation (range vs performance)

#### Examples of strategies:
- Range Optimization Mode → maximize battery life
- Performance Mode → maximize acceleration and power
- Eco Regenerative Mode → aggressive energy recovery

#### The vehicle must switch strategies at runtime without changing core control logic.


## Visitor Pattern Example
### Enterprise Role Governance System
#### Roles represent security principals in an organization.
#### Visitors represent operations performed on those roles.
#### Roles that exist in the system:
- Administrator
- Manager
- Auditor

#### Different operations may be applied later:
- Security audit
- Permission report
- Compliance verification

#### Visitor allows adding these operations without modifying the role classes.
#### Example operations:
- Generate a permission report
- Perform a security risk assessment
