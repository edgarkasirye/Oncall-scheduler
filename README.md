In this CSP-Oncall Scheduler;
- You Supply A list of people to Schedule and the total number of day each should have 
- You also supply a list of People and the corresponding days they MUST be scheduled at (if any)
- You also supply a list of People and the corredponding days they CANNOT be scheduled at (if any)

The underlying constraints include;
1. No person should do two consective days (if one is scheduled for today, they cannot be scheduled for either the following day or the day before)
2. The Number of days must not exceed the required total number of days
3. Everyday must have a person scheduled to it
 