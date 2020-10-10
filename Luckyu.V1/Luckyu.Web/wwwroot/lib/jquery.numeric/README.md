# jquery.numeric
Restrict input fields to numeric input, and a generic jQuery `valueAsNumber`

Sample usage:

```
$('input[type=text]').numeric({ negative: true, decimal: false })
```

To fetch a value of type Number, call `valueAsNumber()`. (This works on `input[type=number]` also.)  

I.e.
```
$('#my-input').valueAsNumber()
```