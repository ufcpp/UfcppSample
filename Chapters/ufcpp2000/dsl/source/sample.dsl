let f(x, y) = x * y;
let g(x, y, z)
{
    let f(x, y) = x + y;
    let t = f(x, y);
    z * t;
}

let x = 1 + 2 * 3;
let y = (-3 + 2 * 4) * f(1, 2);
let z = g(x, y, 10);

x + y + z;
