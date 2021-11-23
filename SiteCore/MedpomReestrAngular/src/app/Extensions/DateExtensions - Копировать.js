const isLeapYear = year => ((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0);
const getDaysInMonth = (year, month) => [31, (isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
Date.prototype.isLeapYear = function () {
    return isLeapYear(this.getFullYear());
};
Date.prototype.isLeapYear = function () {
    return isLeapYear(this.getFullYear());
};
Date.prototype.getDaysInMonth = function () {
    return getDaysInMonth(this.getFullYear(), this.getMonth());
};
Date.prototype.addMonths = function (value) {
    const n = this.getDate();
    this.setDate(1);
    this.setMonth(this.getMonth() + value);
    this.setDate(Math.min(n, this.getDaysInMonth()));
    return this;
};
console.log('hi');
export {};
//# sourceMappingURL=DateExtensions.js.map