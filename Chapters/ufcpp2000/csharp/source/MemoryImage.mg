module Memory
{
    language Memory
    {
        syntax Main = x:Statement+ => Scope{ valuesof(x) };

        syntax Statement =
            x:ValueStatement => x |
            x:ReferenceStatement => x |
            x:ScopeStatement => x;

        syntax ValueStatement =
            i:Identifier '=' v:Integer => ValueStatement { Name = i, Value = v };

        syntax ReferenceStatement =
            i:Identifier '=' New '[' s:Integer ']'
                => ReferenceStatement { Name = i, Size = s } |
            i:Identifier '=' New '[' s:Integer ']'
            '{' c:Identifier+ '}'
                => ReferenceStatement { Name = i, Size = s, Children = c };

        syntax ScopeStatement =
        '{' s:Statement* '}' => Scope { valuesof(s) };

        token Integer = '0'..'9'+;
        token Identifier = ('a'..'z' | 'A'..'Z')+;
        
        @{Classification["Keyword"]} token Var = "var";
        @{Classification["Keyword"]} token New = "new";
        
        interleave WhiteSpace = ' ' | '\t' | '\n' | '\r' | '\r\n';
    }
}
