//creates an object to keep track of values
const Calculator = {
    //This will display 0 on the calculator screen
    Display_Value: '0',
    //this will hold the first operand for any expressions, we set it to null for now
    First_Operand: null,
    //This checks wether or not  the second operand has been inputted by user.
    Wait_Second_Operand: false,
    //This will hold the operator, we set it null for now
    operator: null,
};

//This modifies value each time a button is clicked on
function Input_Digit(digit) {
    const { Display_Value, Wait_Second_Operand } = Calculator;

    // Check if the current Display_Value is a single digit and the Wait_Second_Operand is false
    if (Wait_Second_Operand === true) {
        Calculator.Display_Value = digit;
        Calculator.Wait_Second_Operand = false;
    } else {
        Calculator.Display_Value =
            Display_Value === "0" ? digit : Display_Value + digit;
    }
}

//This section handles decimal points
function Input_Decimal(dot) {
    //this ensures that accidental clicking of the decimal point doesn't
    //cause bugs in the operation
    if (Calculator.Wait_Second_Operand === true) return;
    if (!Calculator.Display_Value.includes(dot)) {
        //we are saying that if the display_value does not contian a decimal point
        //we want to add a decimal point
        Calculator.Display_Value += dot;
    }
}

//this section handles operators
function Handle_Operator(Next_Operator) {
    const {First_Operand, Display_Value, operator} = Calculator;
    //when an operator key is pressed we convert the current number
    //displayed on the screen to a number and then store the result in
    //calculator.first_operand if it doesn't already exist
    const Value_of_Input = parseFloat(Display_Value);
    //checks if an operator already exists and if Wait_Second_Operand is true,
    //then updates the operator an exits from the function
    if (operator && Calculator.Wait_Second_Operand) {
        Calculator.operator = Next_Operator;
        return;
    }
    if (First_Operand == null) {
        Calculator.First_Operand = Value_of_Input;
    } else if (operator) {  //checks if an operator already exists
        const Value_Now = First_Operand || 0;
        //if operator exists, property lookup is performed for the operator
        //in the perform_Calculation object and the function that matches the
        //operator is executed
        let result = Perform_Calculation[operator](Value_Now, Value_of_Input);
        //here we add a fixed amount of numbers after the decimal
        result = Number(result).toFixed (9);
        //this will remove any trailing 0's
        result = (result *1).toString();
        Calculator.Display_Value = parseFloat(result);
        Calculator.First_Operand = parseFloat(result);
    }
    Calculator.Wait_Second_Operand = true;
    Calculator.operator = Next_Operator;
    Update_Display();
    return;
}
const Perform_Calculation = {
    '/': (First_Operand, Second_Operand) => First_Operand / Second_Operand,
    '*': (First_Operand, Second_Operand) => First_Operand * Second_Operand,
    '+': (First_Operand, Second_Operand) => First_Operand + Second_Operand,
    '-': (First_Operand, Second_Operand) => First_Operand - Second_Operand,
    '=': (First_Operand, Second_Operand) => Second_Operand
};
function Calculator_Reset() {
    Calculator.Display_Value = '0';
    Calculator.First_Operand = null;
    Calculator.Wait_Second_Operand = false;
    Calculator.operator = null;
}
//this function updates the calculator screen with the contents of Display_Value
function Update_Display() {
    //makes use of the calculator screen with the contents of Display_Value
    //input tag in the HTML document
    const display = document.querySelector('.calculator-screen');
    display.value = Calculator.Display_Value;
}

Update_Display();
//this section monitors button clicks
const keys = document.querySelector('.calculator-keys');
keys.addEventListener('click', (event) => {
    //the target variable is an object that represents the element 
    //that was clicked
    const { target } = event;
    //if the element that was click on is not a button, exit the function
    if (!target.matches('button')) {
        return;
    }
    if (target.classList.contains('operator')) {
        Handle_Operator(target.value);
        Update_Display();
        return;
    }
    if (target.classList.contains('decimal')) {
        Input_Decimal(target.value);
        Update_Display();
        return;
    }
    //Ensures that AC clears all inputs from the Calculator screen
    if (target.classList.contains('all-clear')) {
        Calculator_Reset();
        Update_Display();
        return;
    }
    // Add the following condition to handle the equal sign button
    if (target.classList.contains('equal-sign')) {
        Handle_Operator(target.value);
        return;
    }
    Input_Digit(target.value);
    Update_Display();
})

function Input_Decimal(dot) {
    if (Calculator.Wait_Second_Operand === true) return;
    if (!Calculator.Display_Value.includes(dot)) {
        Calculator.Display_Value += dot;
    }
    // Add the following condition to remove the leading '0'
    if (Calculator.Display_Value === '0' && dot !== '.') {
        Calculator.Display_Value = dot;
    }
}
