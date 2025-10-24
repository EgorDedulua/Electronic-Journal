// Основная логика студентской части
class StudentDashboard {
    constructor() {
        this.init();
    }

    async init() {
        try {
            // Загружаем все данные
            await StudentAPI.loadAllData();

            // Инициализируем интерфейс
            this.updateUserInfo();
            this.populateSubjectFilter();
            this.renderMarksTable();
            this.renderAbsencesTable();
            this.updateStatistics();
        } catch (error) {
            console.error('Ошибка инициализации:', error);
            Utils.showError('gradesError', 'Не удалось загрузить данные. Попробуйте обновить страницу.');
        }
    }

    updateUserInfo() {
        // В реальном приложении эти данные должны приходить из API
        document.getElementById('studentName').textContent = 'Иванов Иван Иванович';
        document.getElementById('studentGroup').textContent = 'Группа: Т-393';
    }

    populateSubjectFilter() {
        const filter = document.getElementById('subjectFilter');
        const subjects = StudentAPI.getSubjects();

        subjects.forEach(subject => {
            const option = DOMUtils.createElement('option', '', subject.name);
            option.value = subject.name;
            filter.appendChild(option);
        });
    }

    renderMarksTable(filteredMarks = null) {
        const marksToShow = filteredMarks || StudentAPI.getMarks();
        const tbody = document.getElementById('studentTableBody');

        Utils.hideLoading('gradesLoading');
        Utils.hideError('gradesError');

        if (marksToShow.length === 0) {
            DOMUtils.hideElement('studentTable');
            Utils.showError('gradesError', 'Нет оценок для отображения');
            return;
        }

        DOMUtils.clearElement('studentTableBody');

        marksToShow.forEach(mark => {
            const row = DOMUtils.createElement('tr');
            row.innerHTML = `
                <td>${mark.subjectName}</td>
                <td><span class="grade-value">${mark.value}</span></td>
                <td>${mark.teacherName}</td>
                <td>${Utils.formatDate(mark.date)}</td>
            `;
            tbody.appendChild(row);
        });

        DOMUtils.showElement('studentTable');
    }

    renderAbsencesTable() {
        const absences = StudentAPI.getAbsences();
        const tbody = document.getElementById('studentAttendanceTableBody');

        Utils.hideLoading('absencesLoading');
        Utils.hideError('absencesError');

        if (absences.length === 0) {
            DOMUtils.hideElement('studentAttendanceTable');
            Utils.showError('absencesError', 'Нет пропусков для отображения');
            return;
        }

        DOMUtils.clearElement('studentAttendanceTableBody');

        absences.forEach(absence => {
            const row = DOMUtils.createElement('tr');
            row.innerHTML = `
                <td>${absence.subjectName}</td>
                <td>${Utils.formatDate(absence.date)}</td>
                <td>${absence.teacherName}</td>
            `;
            tbody.appendChild(row);
        });

        DOMUtils.showElement('studentAttendanceTable');
    }

    filterGrades() {
        const selectedSubject = document.getElementById('subjectFilter').value;
        const filteredMarks = StudentAPI.getMarksBySubject(selectedSubject);
        this.renderMarksTable(filteredMarks);
    }

    updateStatistics() {
        const marks = StudentAPI.getMarks();
        const absences = StudentAPI.getAbsences();

        // Общее количество оценок
        document.getElementById('totalGrades').textContent = marks.length;

        // Средний балл
        const averageGrade = Utils.calculateAverageGrade(marks);
        document.getElementById('avgGrade').textContent = averageGrade;

        // Количество пропусков
        document.getElementById('totalAbsences').textContent = absences.length;
    }
}

// Глобальные функции
function logout() {
    // В реальном приложении здесь была бы очистка сессии
    if (confirm('Вы уверены, что хотите выйти?')) {
        // window.location.href = 'login.html';
        alert('Выход из системы');
    }
}

function filterGrades() {
    studentDashboard.filterGrades();
}

// Инициализация при загрузке страницы
let studentDashboard;

document.addEventListener('DOMContentLoaded', function () {
    studentDashboard = new StudentDashboard();

    // Обработка глобальных ошибок
    window.addEventListener('error', function (e) {
        console.error('Global error:', e.error);
        Utils.showError('gradesError', 'Произошла ошибка при загрузке данных');
    });
});