// JS扩展方法以及一些公用方法

//重新随机函数
// function random(min, max) {
//     return (Math.random() * (max - min) + min);
// }
function random(min, max) {
    return Math.floor(Math.random() * (max - min + 1) + min);
}