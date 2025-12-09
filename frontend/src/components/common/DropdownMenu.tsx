import React, { useState, useRef, useEffect, useCallback } from 'react';
import { createPortal } from 'react-dom';
import { MoreVertical } from 'lucide-react';

export interface DropdownMenuItem {
  label: string;
  sublabel?: string;
  icon?: React.ReactNode;
  iconBgColor?: string;
  textColor?: string;
  onClick: () => void;
  dividerBefore?: boolean;
}

interface DropdownMenuProps {
  items: DropdownMenuItem[];
  triggerClassName?: string;
  menuWidth?: string;
}

export function DropdownMenu({ items, triggerClassName, menuWidth = 'w-52' }: DropdownMenuProps) {
  const [isOpen, setIsOpen] = useState(false);
  const [position, setPosition] = useState<{ top: number; left: number }>({ top: 0, left: 0 });
  const [openUpward, setOpenUpward] = useState(false);
  const [transformOrigin, setTransformOrigin] = useState('top right');
  const triggerRef = useRef<HTMLButtonElement>(null);
  const menuRef = useRef<HTMLDivElement>(null);

  // Calcula a posição do menu
  const calculatePosition = useCallback(() => {
    if (!triggerRef.current) return;

    const triggerRect = triggerRef.current.getBoundingClientRect();
    const menuWidthPx = 208; // w-52 = 13rem = 208px
    const padding = 8;

    // Estima altura do menu baseado na quantidade de itens
    const itemHeight = 56; // altura aproximada de cada item
    const menuPadding = 16; // padding vertical do menu
    const estimatedMenuHeight = items.length * itemHeight + menuPadding;

    const spaceBelow = window.innerHeight - triggerRect.bottom;
    const spaceAbove = triggerRect.top;
    const spaceRight = window.innerWidth - triggerRect.right;

    // Decide se abre para cima ou para baixo
    const openUpward = spaceBelow < estimatedMenuHeight && spaceAbove > spaceBelow;

    // Decide se alinha à esquerda ou à direita
    const alignLeft = spaceRight < menuWidthPx;

    let top: number;
    let transformOrigin: string;

    if (openUpward) {
      // Abre para cima: posiciona o BOTTOM do menu acima do trigger
      top = triggerRect.top - padding;
      transformOrigin = 'bottom right';
    } else {
      // Abre para baixo: posiciona o TOP do menu abaixo do trigger
      top = triggerRect.bottom + padding;
      transformOrigin = 'top right';
    }

    const left = alignLeft
      ? triggerRect.right - menuWidthPx
      : triggerRect.left;

    setPosition({ top, left });
    setOpenUpward(openUpward);
    setTransformOrigin(transformOrigin);
  }, [items.length]);

  // Abre/fecha o menu
  const toggleMenu = useCallback(() => {
    if (!isOpen) {
      calculatePosition();
    }
    setIsOpen(!isOpen);
  }, [isOpen, calculatePosition]);

  // Fecha o menu
  const closeMenu = useCallback(() => {
    setIsOpen(false);
  }, []);

  // Fecha ao clicar fora
  useEffect(() => {
    if (!isOpen) return;

    const handleClickOutside = (event: MouseEvent) => {
      if (
        menuRef.current &&
        !menuRef.current.contains(event.target as Node) &&
        triggerRef.current &&
        !triggerRef.current.contains(event.target as Node)
      ) {
        closeMenu();
      }
    };

    const handleScroll = () => {
      closeMenu();
    };

    const handleEscape = (event: KeyboardEvent) => {
      if (event.key === 'Escape') {
        closeMenu();
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    window.addEventListener('scroll', handleScroll, true);
    document.addEventListener('keydown', handleEscape);

    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
      window.removeEventListener('scroll', handleScroll, true);
      document.removeEventListener('keydown', handleEscape);
    };
  }, [isOpen, closeMenu]);

  // Recalcula posição ao redimensionar
  useEffect(() => {
    if (!isOpen) return;

    const handleResize = () => {
      calculatePosition();
    };

    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, [isOpen, calculatePosition]);

  const handleItemClick = (item: DropdownMenuItem) => {
    closeMenu();
    item.onClick();
  };

  // Renderiza o menu usando Portal para evitar problemas de overflow
  const renderMenu = () => {
    if (!isOpen) return null;

    const menuContent = (
      <>
        {/* Overlay transparente para capturar cliques */}
        <div 
          className="fixed inset-0 z-[9998]" 
          onClick={closeMenu}
          aria-hidden="true"
        />
        
        {/* Menu */}
        <div
          ref={menuRef}
          className={`fixed z-[9999] ${menuWidth} bg-[var(--surface)] rounded-xl shadow-2xl border border-[var(--border)] py-2 animate-in fade-in zoom-in-95 duration-150`}
          style={{
            top: openUpward ? 'auto' : position.top,
            bottom: openUpward ? `${window.innerHeight - position.top}px` : 'auto',
            left: position.left,
            transformOrigin: transformOrigin,
          }}
          role="menu"
          aria-orientation="vertical"
        >
          {items.map((item, index) => (
            <React.Fragment key={index}>
              {item.dividerBefore && (
                <div className="border-t border-[var(--border)] my-1" />
              )}
              <button
                onClick={() => handleItemClick(item)}
                className={`w-full text-left px-4 py-3 text-sm hover:bg-[var(--surface-muted)] flex items-center gap-3 transition-colors ${
                  item.textColor || 'text-gray-700'
                }`}
                role="menuitem"
              >
                {item.icon && (
                  <div className={`w-8 h-8 rounded-lg flex items-center justify-center ${item.iconBgColor || 'bg-gray-100'}`}>
                    {item.icon}
                  </div>
                )}
                <div>
                  <p className="font-medium">{item.label}</p>
                  {item.sublabel && (
                    <p className={`text-xs ${item.textColor ? item.textColor.replace('text-', 'text-').replace('600', '400').replace('700', '400') : 'text-gray-400'}`}>
                      {item.sublabel}
                    </p>
                  )}
                </div>
              </button>
            </React.Fragment>
          ))}
        </div>
      </>
    );

    return createPortal(menuContent, document.body);
  };

  return (
    <>
      <button
        ref={triggerRef}
        onClick={toggleMenu}
        className={triggerClassName || 'p-2 text-gray-400 hover:text-[var(--text-muted)] hover:bg-gray-100 rounded-lg transition-colors'}
        aria-haspopup="true"
        aria-expanded={isOpen}
      >
        <MoreVertical className="w-5 h-5" />
      </button>
      {renderMenu()}
    </>
  );
}
