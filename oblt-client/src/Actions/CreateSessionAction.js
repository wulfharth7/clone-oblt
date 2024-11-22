//A session gets created for every user.
//Throughout the app, if a user has started using the app and has a session, the rest of the API Calls are requested with this specific session.

export const createSession = async () => {
    try {
        const ipResponse = await fetch('https://api64.ipify.org?format=json');
        const { ip } = await ipResponse.json();

        const userAgent = navigator.userAgent;
        let browserName = 'Unknown';
        let browserVersion = 'Unknown';

        if (userAgent.includes('Chrome')) {
            browserName = 'Chrome';
            browserVersion = userAgent.match(/Chrome\/([\d.]+)/)[1];
        } else if (userAgent.includes('Firefox')) {
            browserName = 'Firefox';
            browserVersion = userAgent.match(/Firefox\/([\d.]+)/)[1];
        } else if (userAgent.includes('Safari') && !userAgent.includes('Chrome')) {
            browserName = 'Safari';
            browserVersion = userAgent.match(/Version\/([\d.]+)/)[1];
        } else if (userAgent.includes('Edge')) {
            browserName = 'Edge';
            browserVersion = userAgent.match(/Edg\/([\d.]+)/)[1];
        } else if (userAgent.includes('Trident') || userAgent.includes('MSIE')) {
            browserName = 'Internet Explorer';
            browserVersion = userAgent.match(/(?:MSIE |rv:)([\d.]+)/)[1];
        }

        const requestBody = {
            connection: {
                "ip-address": ip,
            },
            browser: {
                name: browserName,
                version: browserVersion,
            },
        };

        const response = await fetch('https://localhost:7046/api/Session/create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: 'include',
            body: JSON.stringify(requestBody),
        });

        console.log(JSON.stringify(requestBody));
        if (!response.ok) {
            throw new Error(`Failed to create session: ${response.statusText}`);
        }

        const data = await response.json();
        if (data.status === 'Success' && data.data) {
            sessionStorage.setItem('session-id', data.data['session-id']);
            sessionStorage.setItem('device-id', data.data['device-id']);
            return true;
        } else {
            console.error('Failed to create session:', data.message);
            return false;
        }
    } catch (error) {
        console.error('Error creating session:', error);
        return false;
    }
};
