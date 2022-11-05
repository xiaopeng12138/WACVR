#include <windows.h>

#include <process.h>
#include <limits.h>
#include <stdbool.h>
#include <stdint.h>
#include <process.h>

#include "mercuryio/mercuryio.h"
#include "mercuryio/config.h"
#include "mercuryhook/elisabeth.h"

#include "util/dprintf.h"

static unsigned int __stdcall mercury_io_touch_thread_proc(void *ctx);

static uint8_t mercury_opbtn;
static uint8_t mercury_gamebtn;
static struct mercury_io_config mercury_io_cfg;
static bool mercury_io_touch_stop_flag;
static HANDLE mercury_io_touch_thread;

struct IPCMemoryInfo
{
    uint8_t testBtn;
    uint8_t serviceBtn;
    uint8_t coinBtn;
    uint8_t cardRead;
    uint8_t TouchIoStatus[240];
    uint8_t RGBAData[480 * 4];
};

typedef struct IPCMemoryInfo IPCMemoryInfo;
static HANDLE FileMappingHandle;
IPCMemoryInfo* FileMapping;

void initSharedMemory()
{
    dprintf("initSharedMemory\n");
    dprintf("SharedMemory Size: %d\n", (char)sizeof(IPCMemoryInfo));
    if (FileMapping)
        return;
    if ((FileMappingHandle = CreateFileMapping(INVALID_HANDLE_VALUE, 0, PAGE_READWRITE, 0, sizeof(IPCMemoryInfo), "Local\\WACVR_SHARED_BUFFER")) == 0)
    {
        dprintf("FileMappingHandle Error\n");
        return;
    }
        
    if ((FileMapping = (IPCMemoryInfo*)MapViewOfFile(FileMappingHandle, FILE_MAP_ALL_ACCESS, 0, 0, sizeof(IPCMemoryInfo))) == 0)
    {
        dprintf("FileMapping Error\n");
        return;
    }
        
    memset(FileMapping, 0, sizeof(IPCMemoryInfo));
    SetThreadExecutionState(ES_DISPLAY_REQUIRED | ES_CONTINUOUS);
}

uint16_t mercury_io_get_api_version(void)
{
    return 0x0100;
}

HRESULT mercury_io_init(void)
{
    mercury_io_config_load(&mercury_io_cfg, L".\\segatools.ini");

    initSharedMemory();

    return S_OK;
}

HRESULT mercury_io_poll(void)
{
    mercury_opbtn = 0;
    mercury_gamebtn = 0;

    if ((FileMapping && FileMapping->testBtn) || GetAsyncKeyState(mercury_io_cfg.vk_test)) {
        mercury_opbtn |= MERCURY_IO_OPBTN_TEST;
    }

    if ((FileMapping && FileMapping->serviceBtn) || GetAsyncKeyState(mercury_io_cfg.vk_service)) {
        mercury_opbtn |= MERCURY_IO_OPBTN_SERVICE;
    }

    if ((FileMapping && FileMapping->coinBtn) || GetAsyncKeyState(mercury_io_cfg.vk_coin)) {
        mercury_opbtn |= MERCURY_IO_OPBTN_COIN;
    }

    if (GetAsyncKeyState(mercury_io_cfg.vk_vol_up)) {
        mercury_gamebtn |= MERCURY_IO_GAMEBTN_VOL_UP;
    }

    if (GetAsyncKeyState(mercury_io_cfg.vk_vol_down)) {
        mercury_gamebtn |= MERCURY_IO_GAMEBTN_VOL_DOWN;
    }

    return S_OK;
}

void mercury_io_get_opbtns(uint8_t *opbtn)
{
    if (opbtn != NULL) {
        *opbtn = mercury_opbtn;
    }
}

void mercury_io_get_gamebtns(uint8_t *gamebtn)
{
    if (gamebtn != NULL) {
        *gamebtn = mercury_gamebtn;
    }
}

HRESULT mercury_io_touch_init(void)
{
    return S_OK;
}

void mercury_io_touch_start(mercury_io_touch_callback_t callback)
{
    if (mercury_io_touch_thread != NULL) {
        return;
    }

    mercury_io_touch_thread = (HANDLE) _beginthreadex(
        NULL,
        0,
        mercury_io_touch_thread_proc,
        callback,
        0,
        NULL
    );
}

void mercury_io_touch_set_leds(struct led_data data)
{
    if (FileMapping)
    {
        data.rgba[3] = 0xFF; //IPC idle flag
        memcpy(FileMapping->RGBAData, data.rgba, 480 * 4);
    }
}

static unsigned int __stdcall mercury_io_touch_thread_proc(void *ctx)
{
    mercury_io_touch_callback_t callback;
    bool cellPressed[240];

    callback = ctx;

    while (!mercury_io_touch_stop_flag) {
        if (FileMapping) {
            memcpy(cellPressed, FileMapping->TouchIoStatus, 240);
        }
        callback(cellPressed);
        //Sleep(1);
    }

    data.rgba[3] = 0x00; //IPC idle flag

    return 0;
}
