// Утилитарные функции
class Utils {
    static formatDate(dateString) {
        try {
            // Преобразуем разные форматы дат (с точками и дефисами)
            const normalizedDate = dateString.replace(/\./g, '-');
            const date = new Date(normalizedDate);

            if (isNaN(date.getTime())) {
                return dateString; // Возвращаем исходную строку если не удалось распарсить
            }

            return date.toLocaleDateString('ru-RU', {
                day: '2-digit',
                month: '2-digit',
                year: 'numeric'
            });
        } catch (error) {
            console.error('Ошибка форматирования даты:', error);
            return dateString;
        }
    }

    static showError(elementId, message) {
        const errorElement = document.getElementById(elementId);
        if (errorElement) {
            errorElement.textContent = message;
            errorElement.style.display = 'block';
        }
    }

    static hideError(elementId) {
        const errorElement = document.getElementById(elementId);
        if (errorElement) {
            errorElement.style.display = 'none';
        }
    }

    static showLoading(elementId) {
        const loadingElement = document.getElementById(elementId);
        if (loadingElement) {
            loadingElement.style.display = 'block';
        }
    }

    static hideLoading(elementId) {
        const loadingElement = document.getElementById(elementId);
        if (loadingElement) {
            loadingElement.style.display = 'none';
        }
    }

    static calculateAverageGrade(marks) {
        if (!marks || marks.length === 0) return 0;

        const sum = marks.reduce((total, mark) => total + mark.value, 0);
        return (sum / marks.length).toFixed(1);
    }

    static debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }
}

// Функции для работы с DOM
class DOMUtils {
    static createElement(tag, className, textContent = '') {
        const element = document.createElement(tag);
        if (className) element.className = className;
        if (textContent) element.textContent = textContent;
        return element;
    }

    static clearElement(elementId) {
        const element = document.getElementById(elementId);
        if (element) {
            element.innerHTML = '';
        }
    }

    static showElement(elementId) {
        const element = document.getElementById(elementId);
        if (element) {
            element.style.display = 'block';
        }
    }

    static hideElement(elementId) {
        const element = document.getElementById(elementId);
        if (element) {
            element.style.display = 'none';
        }
    }
}