module StackMachineEmulator
{
    language MiniLanguage
    {
        syntax Main = x:Statement+ => Statements { valuesof(x) };
        
        // Statement

        syntax Statement =
            Nop => null|
            x:LetVariableStatement => x |
            x:LetFunctionStatement => x |
            x:ReturnStatement => x;
        
        syntax Nop = Delimiter;
        
        syntax ReturnStatement =
            x:Expression Delimiter => Return { x };

        syntax LetVariableStatement =
            Let i:Identifier '=' e:Expression Delimiter => Variable { Name = i, Expression = e };
        
        syntax LetFunctionStatement =
            Let i:Identifier '(' args:IdentifierList ')' '{' body:Statement* '}' => Function { Name = i, Arguments = args, Statements = body } |
            Let i:Identifier '(' args:IdentifierList ')'  '=' body:Expression Delimiter => Function { Name = i, Arguments = args, Statements = Return { body } } ;
            
        syntax IdentifierList =
            i:Identifier => [i] |
            i:Identifier ',' list:IdentifierList => [i, valuesof(list)] ;

        @{Classification["Keyword"]}
        token Let = 'let';
        
        // Expression
        
        syntax Expression =
            x:ConditionalExpression => x ;
        
        syntax ExpressionList =
            e:Expression => [e] |
            e:Expression ',' list:ExpressionList => [e, valuesof(list)] ;
        
        syntax ConditionalExpression =
            x:LogicalOrExpression => x |
            cond:LogicalOrExpression '?' t:Expression ':' f:Expression => Conditional { Condition{cond}, TrueExpression{t}, FalseExpression{f} };

        syntax LogicalOrExpression =
            x:LogicalAndExpression => x |
            l:LogicalOrExpression '|' r:LogicalAndExpression => Binary { Operator{'|'}, Left {l}, Right{r} };
        syntax LogicalAndExpression =
            x:EqualityExpression => x |
            l:LogicalAndExpression '&' r:EqualityExpression => Binary { Operator{'&'}, Left {l}, Right{r} };

        syntax EqualityExpression =
            x:RelationalExpression => x |
            l:EqualityExpression '==' r:RelationalExpression => Binary { Operator{'=='}, Left {l}, Right{r} } |
            l:EqualityExpression '!=' r:RelationalExpression => Binary { Operator{'!='}, Left {l}, Right{r} };
        syntax RelationalExpression =
            x:AdditiveExpression => x |
            l:RelationalExpression '<' r:AdditiveExpression => Binary { Operator{'<'}, Left {l}, Right{r} } |
            l:RelationalExpression '>' r:AdditiveExpression => Binary { Operator{'>'}, Left {l}, Right{r} } |
            l:RelationalExpression '<=' r:AdditiveExpression => Binary { Operator{'<='}, Left {l}, Right{r} } |
            l:RelationalExpression '>=' r:AdditiveExpression => Binary { Operator{'>='}, Left {l}, Right{r} };

        syntax AdditiveExpression =
            x:MultiplicativeExpression => x |
            l:AdditiveExpression '+' r:MultiplicativeExpression => Binary { Operator{'+'}, Left {l}, Right{r} } |
            l:AdditiveExpression '-' r:MultiplicativeExpression => Binary { Operator{'-'}, Left {l}, Right{r} };
        syntax MultiplicativeExpression =
            x:UnaryExpression => x |
            l:MultiplicativeExpression '*' r:UnaryExpression => Binary { Operator{'*'}, Left {l}, Right{r} } |
            l:MultiplicativeExpression '/' r:UnaryExpression => Binary { Operator{'/'}, Left {l}, Right{r} } ;
        syntax UnaryExpression =
            x:PrimaryExpression => x |
            '+' x:PrimaryExpression => Unary { Operator{'+'}, Expression {x} } |
            '-' x:PrimaryExpression => Unary { Operator{'-'}, Expression {x} } ;
        syntax CallExpression =
            f:Identifier '(' params:ExpressionList ')' => Call { Function = f, Parameters = params };
        syntax PrimaryExpression =
            x:Identifier => Parameter { x } |
            x:Integer => Constant { x } |
            '(' x:Expression ')' => x |
            x:CallExpression => x;

        token Integer = '0'..'9'+;
        token Identifier = ('a'..'z' | 'A'..'Z')+;
        token Delimiter = ';';

        interleave WhiteSpace = ' ' | '\t' | '\n' | '\r' | '\r\n';
    }
}
