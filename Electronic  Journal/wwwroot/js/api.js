const API_BASE_URL = 'https://localhost:7287';
const STUDENT_ID = 1;

// Глобальные переменные для данных
let allMarks = [];
let allSubjects = [];
let allAbsences = [];

// API функции
class StudentAPI {
    static async loadMarks() {
        try {
            const response = await fetch(`${API_BASE_URL}/Student/${STUDENT_ID}/marks`);
            if (!response.ok) throw new Error('Ошибка загрузки оценок');
            allMarks = await response.json();
            return allMarks;
        } catch (error) {
            console.error('Ошибка загрузки оценок:', error);
            throw error;
        }
    }

    static async loadSubjects() {
        try {
            const response = await fetch(`${API_BASE_URL}/Student/subjects/${STUDENT_ID}`);
            if (!response.ok) throw new Error('Ошибка загрузки предметов');
            allSubjects = await response.json();
            return allSubjects;
        } catch (error) {
            console.error('Ошибка загрузки предметов:', error);
            throw error;
        }
    }

    static async loadAbsences() {
        try {
            const response = await fetch(`${API_BASE_URL}/Student/${STUDENT_ID}/absenses`);
            if (!response.ok) throw new Error('Ошибка загрузки пропусков');
            allAbsences = await response.json();
            return allAbsences;
        } catch (error) {
            console.error('Ошибка загрузки пропусков:', error);
            throw error;
        }
    }

    static async loadAllData() {
        try {
            await Promise.all([
                this.loadMarks(),
                this.loadSubjects(),
                this.loadAbsences()
            ]);
            return {
                marks: allMarks,
                subjects: allSubjects,
                absences: allAbsences
            };
        } catch (error) {
            console.error('Ошибка загрузки всех данных:', error);
            throw error;
        }
    }

    // Геттеры для доступа к данным
    static getMarks() {
        return allMarks;
    }

    static getSubjects() {
        return allSubjects;
    }

    static getAbsences() {
        return allAbsences;
    }

    static getMarksBySubject(subjectName) {
        if (!subjectName) return allMarks;
        return allMarks.filter(mark => mark.subjectName === subjectName);
    }
}