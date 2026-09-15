import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class ToastService {
    private activeToast: HTMLElement | null = null;

    show(
        message: string,
        type: 'success' | 'error' | 'warning' = 'success',
        duration = 4000
    ) {

        const toastContainer = document.getElementById('toast-container') || this.createContainer();

        //remove previous toast (prevents firing multiples)
        if (this.activeToast) {
            this.activeToast.remove();
            this.activeToast = null;
        }

        const toast = document.createElement('div');

        toast.className = `
            toast align-items-center text-white 
            bg-${this.getColor(type)} border-0 show
        `;

        toast.style.display = 'inline-block';
        toast.style.width = 'auto';
        toast.style.whiteSpace = 'normal';
        toast.style.minWidth = 'unset';
        toast.style.maxWidth = '380px';
        toast.style.boxShadow = '0 6px 18px rgba(0,0,0,0.15)';
        toast.style.marginBottom = '10px';
        toast.style.transition = 'all 0.3s ease';

        toast.setAttribute('role', 'alert');
        toast.setAttribute('aria-live', 'assertive');
        toast.setAttribute('aria-atomic', 'true');

        toast.innerHTML = `
            <div class="toast-body py-2 px-3 d-flex align-items-center" style="min-height: 44px;">
                ${message}
            </div>
        `;

        toastContainer.appendChild(toast);

        this.activeToast = toast;

        //auto remove
        setTimeout(() => {
            if (this.activeToast === toast) {
                toast.remove();
                this.activeToast = null;
            }
        }, duration);
    }

    private getColor(type: string): string {
        switch (type) {
            case 'error': return 'danger';
            case 'warning': return 'warning';
            default: return 'success';
        }
    }

    private createContainer() {
        const container = document.createElement('div');

        container.id = 'toast-container';

        container.className = `
            position-fixed top-0 end-0 p-3
            d-flex flex-column align-items-end gap-2
        `;

        container.style.zIndex = '1055';

        document.body.appendChild(container);

        return container;
    }
}