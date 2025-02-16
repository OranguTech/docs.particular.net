 * `SubscriptionNamingConvention(Func<string, string>)`: By default subscription names are derived from the endpoint name. This callback allows for a replacement name for the subscription. Subscription names must adhere to the limits outlined in [the Microsoft documentation on subscription creation](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quotas).
 * `SubscriptionRuleNamingConvention(Func<Type, string>)`: By default rule names are derived from the message type's full name. This callback allows for a replacement name for the rule. Rule names must adhere to the limits outlined in [Service Bus quotas](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quotas).

#### Combining shorteners and naming conventions

When both a shortener and a naming convention are provided for a subscription or subscription rule, the naming convention is applied first, and the result is then passed into the shortener.