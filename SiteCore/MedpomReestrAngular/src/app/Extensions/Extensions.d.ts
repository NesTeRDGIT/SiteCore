declare global {

    interface Date {
        addMonths(month: number): Date;
        isLeapYear(): boolean;
        getDaysInMonth(): number;
        toASPstring(): string;
    }
}

export {}