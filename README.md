# DavidHart
Share brokering demo 
You are going to make it big by playing the stock market – “buy low, sell high” you scream as you sit down at your computer. You’ve heard good things about this company  - so you start investigating its historic share prices. You’re going to need to time your trades so that you make the biggest gain possible! But there’s a catch, as you’re new to the game, your broker will only allow you one trade.
So, you look back at a list of market-opening prices for their stock. You are not into this risky day-trading approach, so are looking at the opening price each day of the last month.
You decide to put a program together (using industry best-practice standards of course!) using your favourite language, to help you work out what the single best trade you could have made that month would be, to maximize gain. 
Remember - buy low, sell high. And before you ask… unfortunately your DeLorean won’t start, so you can't sell before you buy.

Input Description
Your program will be provided with a list of the company market-opening stock prices from the beginning of each trading day of the last month. It will be formatted as a comma separated list of two decimal floats (pounds and pence), listed in chronological order from day 1 through to day 30 of the month. Sample datasets are included in this Zip file.
Example:
19.15,18.30,18.88,17.93,15.95,19.03,19.00 etc..

Output Description
Your program should provide the buy day and sell day – so two days of the month and their opening stock prices. So, in chronological order showing when you should buy, and sell, again separated by a comma.
To confirm - the format being buyDayOfMonth(price),sellDayOfMonth(price)
Example:
5(15.95),6(19.03)

