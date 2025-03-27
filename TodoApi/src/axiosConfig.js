
import axios from 'axios';

const axios1 = axios.create();

axios1.interceptors.response.use(
    response => response,
    error => {
        // Displaying the dialog for the error
        const dialog = document.createElement('dialog');
        dialog.textContent = error.message || 'Something went wrong!';
        document.body.appendChild(dialog);

        // Provide a way to close the dialog after a few seconds or on click
        setTimeout(() => {
            dialog.close();
            dialog.remove();
        }, 5000);

        dialog.addEventListener('click', () => {
            dialog.close();
            dialog.remove();
        });

        dialog.showModal();

        return Promise.reject(error);
    }
);

export default axios1;